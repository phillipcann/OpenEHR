namespace Shellscripts.OpenEHR.Configuration
{
    using System;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;    

    using Shellscripts.OpenEHR.Rest;
    using Shellscripts.OpenEHR.Serialisation.Converters;

    /// <summary>
    /// Default application configuration used for testing and console app.
    /// </summary>
    public static class ContainerConfiguration
    {
        public static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            string env = context.HostingEnvironment.EnvironmentName;

            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
            ;

            if (args?.Length > 0)
            {
                builder.AddCommandLine(args);
            }
        }
        
        public static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.ClearProviders();

            if (context.HostingEnvironment.IsDevelopment())
            {
                builder
                    .AddConsole()
                    .AddDebug();
            }
            else
            {
                // TODO : File Logging
            }
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {            
            //services.AddTransient(provider =>
            //{
            //    var config = provider.GetRequiredService<IConfiguration>();
            //    var systemUri = config.GetSection("HttpClients").GetValue<string>("EhrClient1:SystemUri", string.Empty);
            //    return new UriAppendingHandler(systemUri);
            //});

            services.AddHttpClient<IEhrClient, EhrClient>((services, client) =>
            {
                var config = services.GetRequiredService<IConfiguration>();
                var httpClientConfig = config.GetSection("HttpClients");

                var baseUrl = new Uri(httpClientConfig.GetValue("EhrClient:BaseUrl", string.Empty));
                var timeout = httpClientConfig.GetValue("EhrClient:Timeout", 30);
                var preferType = httpClientConfig.GetValue("EhrClient:PreferType", "minimal");
                var acceptType = httpClientConfig.GetValue("EhrClient:AcceptType", "application/xml");

                client.DefaultRequestHeaders.Add("User-Agent", "Shellscripts.OpenEhr");
                client.DefaultRequestHeaders.Add("Prefer", preferType);
                client.DefaultRequestHeaders.Add("Accept", acceptType);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.BaseAddress = baseUrl;
            });
            //.AddHttpMessageHandler<UriAppendingHandler>();

            // Singletons
            services.AddSingleton<DvDateTimeConverter>();
            services.AddSingleton(provider =>
            {
                var options = new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                };

                options.Converters.Add(provider.GetRequiredService<DvDateTimeConverter>());
                return options;
            });
        }
    }
}
