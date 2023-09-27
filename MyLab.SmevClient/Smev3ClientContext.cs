using System;
using System.Net.Http;

namespace MyLab.SmevClient
{
    internal class Smev3ClientContext: ISmev3ClientContext
    {
        internal Smev3ClientContext(
            IHttpClientFactory httpClientFactory, 
            SmevServiceConfig smevServiceConfig)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            SmevServiceConfig = smevServiceConfig ?? throw new ArgumentNullException(nameof(smevServiceConfig));
        }

        /// <summary>
        /// Фабрика клиреез клиента
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get;  }

        /// <summary>
        /// Конфигурация сервиса СМЭВ
        /// </summary>
        public SmevServiceConfig SmevServiceConfig { get; }
    }
}
