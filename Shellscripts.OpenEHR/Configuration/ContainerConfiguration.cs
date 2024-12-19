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
                // TODO : Add some file Logging. Serilog?
            }
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // HttpClient
            services.AddHttpClient<IEhrClient, EhrClient>((services, client) =>
            {
                var config = services.GetRequiredService<IConfiguration>();
                var httpClientConfig = config.GetSection("HttpClients");

                var baseUrl = new Uri(httpClientConfig.GetValue("EhrClient:BaseUrl", string.Empty));
                var timeout = httpClientConfig.GetValue("EhrClient:Timeout", 30);
                var preferType = httpClientConfig.GetValue("EhrClient:PreferType", "minimal");
                var acceptType = httpClientConfig.GetValue("EhrClient:AcceptType", "application/json");

                client.DefaultRequestHeaders.Add("User-Agent", "Shellscripts.OpenEhr");
                client.DefaultRequestHeaders.Add("Prefer", preferType);
                client.DefaultRequestHeaders.Add("Accept", acceptType);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.BaseAddress = baseUrl;
            });

            
            
            // JsonConverters
            services.AddSingleton<DataValueConverter>();
            services.AddSingleton<DvDateTimeConverter>();
            services.AddSingleton<ObjectRefConverter>();
            services.AddSingleton<PartyProxyConverter>();
            services.AddSingleton<ItemStructureConverter>();
            services.AddSingleton<ContentItemConverter>();

            services.AddSingleton<ItemArrayConverter>();
            services.AddSingleton<EventArrayConverter>();


            services.AddSingleton(provider =>
            {
                var options = new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,                    
                    WriteIndented = true,

                    IgnoreReadOnlyFields = true,
                    IgnoreReadOnlyProperties = true
                };
                
                options.Converters.Add(provider.GetRequiredService<DataValueConverter>());
                options.Converters.Add(provider.GetRequiredService<DvDateTimeConverter>());
                options.Converters.Add(provider.GetRequiredService<ObjectRefConverter>());
                options.Converters.Add(provider.GetRequiredService<PartyProxyConverter>());
                options.Converters.Add(provider.GetRequiredService<ItemStructureConverter>());                
                options.Converters.Add(provider.GetRequiredService<ContentItemConverter>());

                options.Converters.Add(provider.GetRequiredService<ItemArrayConverter>());
                options.Converters.Add(provider.GetRequiredService<EventArrayConverter>());

                return options;
            });
        }
    }
}
