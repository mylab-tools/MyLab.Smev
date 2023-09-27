using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.SmevClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void UseSmev3Client(this IServiceCollection serviceCollection)
        {
            var httpClientBuilder = serviceCollection.AddHttpClient("SmevClient", (serviceProvider, httpClient) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();

                httpClient.BaseAddress = new Uri(config["Smev:Url"]);
            });

            serviceCollection.AddSingleton<ISmev3ClientFactory>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                
                var servicesConfigs = config.GetSection("Smev:Services")
                                                .Get<Dictionary<string, SmevServiceConfig>>();
                
                var httpClientFactory = serviceProvider
                                                .GetRequiredService<IHttpClientFactory>();

                return new Smev3ClientFactory(httpClientFactory, servicesConfigs);
            });
        }
    }
}
