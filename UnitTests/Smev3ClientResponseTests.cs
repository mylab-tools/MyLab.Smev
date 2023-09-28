using System.Net;
using System.Text;
using MyLab.SmevClient;
using MyLab.SmevClient.Smev;
using MyLab.SmevClient.Soap;

namespace UnitTests
{
    public class Smev3ClientResponseTests
    {
        [Fact]
        public async Task ReadShortErrorInfo()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/SoapFaultResponse.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var soapFault = await smevResponse.ReadSoapBodyAsAsync<SoapFault>();

            Assert.NotNull(soapFault.FaultString);
        }

        [Fact]
        public async Task ReadDetailErrorInfo()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/SoapFaultResponse_SignatureVerificationFault.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var soapFault = await smevResponse.ReadSoapBodyAsAsync<SoapFault>();

            Assert.NotNull(soapFault.DetailXmlFragment);
        }

        [Fact]
        public async Task ReadSendRequestResponseMessageId_Exists()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/SendRequestResponse.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse.ReadSoapBodyAsAsync<SendRequestResponse>();

            Assert.NotNull(response.MessageMetadata.MessageId);
        }

        [Fact]
        public async Task ReadSendRequestResponseStatus_requestIsQueued()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/SendRequestResponse.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse.ReadSoapBodyAsAsync<SendRequestResponse>();

            Assert.Equal("requestIsQueued", response.MessageMetadata.Status);
        }

        [Fact]
        public async Task ReadGetResponseResponse_InvalidContent()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/GetResponseResponse_InvalidContent.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse.ReadSoapBodyAsAsync<GetResponseResponse<FakeSmevServiceResponse>>();

            Assert.NotNull(response.ResponseMessage?.Response?.SenderProvidedResponseData?.ProcessingStatus?.Fault);
        }

        [Fact]
        public async Task ReadGetResponseResponse_ServiceResponseExists()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/GetResponseResponse_ValidResponse.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse.ReadSoapBodyAsAsync<GetResponseResponse<FakeSmevServiceResponse>>();

            Assert.NotNull(response.ResponseMessage?.Response?.SenderProvidedResponseData?.MessagePrimaryContent?.Content);
        }

        [Fact]
        public async Task ReadGetResponseResponse_InvalidContent_ServiceResponseIsNull()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/GetResponseResponse_InvalidContent.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse.ReadSoapBodyAsAsync<GetResponseResponse<FakeSmevServiceResponse>>();

            Assert.Null(response.ResponseMessage.Response.SenderProvidedResponseData.MessagePrimaryContent);
        }

        [Fact]
        public async Task ReadGetResponseResponse_EmptyQueue()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(await File.ReadAllTextAsync("TestData/GetResponseResponse_EmptyQueue.xml"))
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse
                .ReadSoapBodyAsAsync<GetResponseResponse<FakeSmevServiceResponse>>()
                ;

            Assert.Null(response.ResponseMessage.Response);
        }

        [Fact]
        public async Task ReadGetResponseResponse_MultipartEmptyQueue()
        {
            var content = new StringContent(await File.ReadAllTextAsync("TestData/GetResponseResponse_MultipartEmptyQueue.xml", Encoding.UTF8));

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/mixed");

            content.Headers.ContentType.Parameters.Add(
                new System.Net.Http.Headers.NameValueHeaderValue("boundary", "f438a15e-9b5b-491f-9b47-aba4d00b8837"));

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

            var smevResponse = new Smev3ClientResponse(httpResponse);

            var response = await smevResponse
                .ReadSoapBodyAsAsync<GetResponseResponse<FakeSmevServiceResponse>>()
                ;

            Assert.Null(response.ResponseMessage.Response);
        }
    }
}
