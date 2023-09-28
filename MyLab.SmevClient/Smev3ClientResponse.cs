using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyLab.SmevClient.Http;
using MyLab.SmevClient.Soap;

namespace MyLab.SmevClient
{
    public class Smev3ClientResponse
    {
        protected HttpResponseMessage HttpResponse;

        public Smev3ClientResponse(HttpResponseMessage response)
        {
            HttpResponse = response ?? throw new ArgumentNullException(nameof(response));
        }

        /// <summary>
        /// Открепляет HTTP ответ. Далее нельзя вызывать никакие методы объекта кроме Dispose
        /// </summary>
        /// <returns></returns>
        internal HttpResponseMessage DetachHttpResponse()
        {
            var response = HttpResponse;

            HttpResponse = null;

            return response;
        }

        /// <summary>
        /// Чтение элемента Body содержимого ответа как тип T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> ReadSoapBodyAsAsync<T>(CancellationToken cancellationToken = default)
            where T : ISoapEnvelopeBody, new()
        {
            return HttpResponse.Content.ReadSoapBodyAsAsync<T>(cancellationToken);
        }

        /// <summary>
        /// Чтение ответа в строку
        /// </summary>
        /// <returns></returns>
        public Task<string> ReadSoapBodyAsStringAsync(CancellationToken cancellationToken = default)
        {
            return HttpResponse.Content.ReadSoapBodyAsStringAsync(cancellationToken);
        }
    }
}
