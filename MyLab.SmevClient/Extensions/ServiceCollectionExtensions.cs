using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyLab.SmevClient.Options;

namespace MyLab.SmevClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmev3Client(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            serviceCollection.AddHttpClient("SmevClient", (serviceProvider, httpClient) =>
            {
                var opts = serviceProvider.GetRequiredService<IOptions<SmevClientOptions>>();
                
                if (opts?.Value.Url == null)
                    throw new InvalidOperationException("Service url is not specified");

                httpClient.BaseAddress = new Uri(opts.Value.Url);
            });

            serviceCollection.AddSingleton<ISmev3Client, Smev3Client>();

            return serviceCollection;
        }

        public static IServiceCollection ConfigureSmev3Client(this IServiceCollection serviceCollection, IConfiguration configuration, string configSectionName = "Smev")
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var configSection = configuration.GetSection(configSectionName);

            return serviceCollection.Configure<SmevClientOptions>(configSection);
        }

        public static IServiceCollection ConfigureSmev3Client(this IServiceCollection serviceCollection, Action<SmevClientOptions> initOptions)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (initOptions == null) throw new ArgumentNullException(nameof(initOptions));


            return serviceCollection.Configure(initOptions);
        }


    }
}
