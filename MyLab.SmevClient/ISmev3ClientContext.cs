using System.Net.Http;

namespace MyLab.SmevClient
{
    internal interface ISmev3ClientContext
    {
        /// <summary>
        /// Фабрика клиента http
        /// </summary>
        IHttpClientFactory HttpClientFactory { get; }

        /// <summary>
        /// Параметры сервиса СМЭВ
        /// </summary>
        SmevServiceConfig SmevServiceConfig { get; }
    }
}
