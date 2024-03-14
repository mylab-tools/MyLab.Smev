using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.ApiClient;
using MyLab.Log.Dsl;
using MyLab.SmevClient.Crypt;
using MyLab.SmevClient.Http;
using MyLab.SmevClient.Options;
using MyLab.SmevClient.Smev;
using MyLab.SmevClient.Soap;
using MyLab.SmevClient.Utils;

namespace MyLab.SmevClient
{
    internal class Smev3Client : ISmev3Client
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Криптоалгоритм
        /// </summary>
        private GostAsymmetricAlgorithm _algorithm;
        
        private readonly IDslLogger _log;
        private readonly HttpMessageDumper _dumper;

        public Smev3Client(IHttpClientFactory httpClientFactory, IOptions<SmevClientOptions> opts, ILogger<Smev3Client> logger = null)
        {
            _httpClientFactory = httpClientFactory;
            var certProvider = new ByTumbprintCertHandleProvider(StoreLocation.CurrentUser, opts.Value.CertThumbprint);
            _algorithm = new GostAsymmetricAlgorithm(certProvider);
            _log = logger?.Dsl();
            _dumper = new HttpMessageDumper();
        }

        /// <summary>
        /// Отправка запроса
        /// </summary>
        /// <typeparam name="TServiceRequest"></typeparam>
        /// <param name="context">Параметры метода</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<Smev3ClientResponse<SendRequestResponse>> SendRequestAsync<TServiceRequest>(SendRequestExecutionContext<TServiceRequest> context,
                                                                      CancellationToken cancellationToken)
            where TServiceRequest : new()
        {
            HttpResponseMessage httpResponse = null;
            try
            {
                var reqData = new SenderProvidedRequestData<TServiceRequest>(
                    messageId: Rfc4122.GenerateUUIDv1(),
                    xmlElementId: "SIGNED_BY_CONSUMER",
                    content: new MessagePrimaryContent<TServiceRequest>(context.RequestData)
                )
                {
                    TestMessage = context.IsTest
                };

                var envelope = new SendRequestRequest<TServiceRequest>
                    (
                        requestData: reqData,
                        signer: new Smev3XmlSigner(_algorithm)
                    );

                if (context.Attachments != null)
                {
                    reqData.AttachmentHeaders = new AttachmentHeaderList(
                        context.Attachments.Select(a => new AttachmentHeader(a.Id, a.MimeType)
                        {
                            SignatureBase64 = a.SignatureBase64
                        })
                    );
                    envelope.Attachments = new AttachmentContentList(
                        context.Attachments.Select(a => new AttachmentContent(a.Id, a.ContentBase64))
                    );
                }
                
                httpResponse = await SendAsync(envelope, cancellationToken);

                var soapEnvelopeBody = await httpResponse
                                                .Content
                                                .ReadSoapBodyAsAsync<SendRequestResponse>(cancellationToken);

                return new Smev3ClientResponse<SendRequestResponse>(httpResponse, soapEnvelopeBody);
            }
            catch
            {
                httpResponse?.Dispose();

                throw;
            }
        }

        /// <summary>
        /// Получение сообщения из очереди входящих ответов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="namespaceUri"></param>
        /// <param name="rootElementLocalName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Smev3ClientResponse> GetResponseAsync(Uri namespaceUri, string rootElementLocalName,
                                                    CancellationToken cancellationToken)
        {
            var envelope = new GetResponseRequest(
                    requestData: new MessageTypeSelector(namespaceUri, rootElementLocalName)
                    {
                        Timestamp = DateTime.Now,
                        Id = "SIGNED_BY_CONSUMER"
                    },
                    signer: new Smev3XmlSigner(_algorithm));

            var httpResponse = await SendAsync(envelope, cancellationToken);

            return new Smev3ClientResponse(httpResponse);
        }

        /// <summary>
        /// Получение сообщения из очереди входящих ответов c десереализацией ответа в тип T
        /// </summary>
        /// <typeparam name="TServiceResponse"></typeparam>
        /// <param name="namespaceUri"></param>
        /// <param name="rootElementLocalName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Smev3ClientResponse<GetResponseResponse<TServiceResponse>>> GetResponseAsync<TServiceResponse>(Uri namespaceUri, string rootElementLocalName,
                                                CancellationToken cancellationToken)
            where TServiceResponse : new()
        {
            var response = await GetResponseAsync(namespaceUri, rootElementLocalName, cancellationToken);

            var data = await response.ReadSoapBodyAsAsync<GetResponseResponse<TServiceResponse>>(cancellationToken);

            return new Smev3ClientResponse<GetResponseResponse<TServiceResponse>>(response.HttpResponse, data);
        }

        /// <summary>
        /// Подтверждение получения ответа
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Smev3ClientResponse<AckResponse>> AckAsync(Guid messageId, bool accepted, CancellationToken cancellationToken)
        {
            var envelope = new AckRequest(
                    new AckTargetMessage
                    {
                        MessageID = messageId,
                        Id = "SIGNED_BY_CALLER",
                        Accepted = accepted
                    },
                    signer: new Smev3XmlSigner(_algorithm));
            
            var httpResponse = await SendAsync(envelope, cancellationToken)
                                        ;

            var data = await httpResponse.Content.ReadSoapBodyAsAsync<AckResponse>(cancellationToken)
                                        ;

            return new Smev3ClientResponse<AckResponse>(httpResponse, data);
        }
        
        public void Dispose()
        {
            _algorithm?.Dispose();
            _algorithm = null;
        }
        
        /// <summary>
        /// Отправка конверта
        /// </summary>
        /// <param name="envelopeBytes"></param>
        /// <param name="cancellationToken"></param>
        private async Task<HttpResponseMessage> SendAsync(ISmev3Envelope envelope, CancellationToken cancellationToken)
        {
            if (envelope == null) throw new ArgumentNullException(nameof(envelope));

            var envelopeBytes = envelope.Get();
            var content = new ByteArrayContent(
                envelopeBytes, 0, envelopeBytes.Length);

            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Xml)
            {
                CharSet = "utf-8"
            };

            content.Headers.Add("SOAPAction", $"\"urn:{envelope.SmevMethod}\"");

            HttpResponseMessage httpResponse = null;
            try
            {
                using var httpClient = _httpClientFactory.CreateClient("SmevClient");

                httpResponse = await httpClient.PostAsync(string.Empty, content, cancellationToken)
                                               ;

                var reqDump = await _dumper.DumpAsync(httpResponse.RequestMessage);
                var respDump = await _dumper.DumpAsync(httpResponse);

                _log?.Action("Request sent")
                    .AndFactIs("request", reqDump)
                    .AndFactIs("response", respDump)
                    .Write();

                if (httpResponse.IsSuccessStatusCode)
                {
                    return httpResponse;
                }

                var faultInfo = await httpResponse.Content.ReadSoapBodyAsAsync<SoapFault>(cancellationToken)
                                                  ;

                throw new Smev3Exception(
                    $"FaultCode: {faultInfo.FaultCode}. FaultString: {faultInfo.FaultString}.")
                {
                    FaultInfo = faultInfo,
                    ResponseMessage = httpResponse
                };
            }
            catch
            {
                httpResponse?.Dispose();

                throw;
            }
        }
    }
}
