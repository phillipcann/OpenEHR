namespace Shellscripts.OpenEHR.Configuration
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Repositories;
    using Shellscripts.OpenEHR.Rest;
    using Shellscripts.OpenEHR.Serialisation;
    using Shellscripts.OpenEHR.Serialisation.Converters;
    using Shellscripts.OpenEHR.Serialisation.Converters.Enumerable;
    using Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable;

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
            // Transients
            services.AddTransient<EhrClientUrlHandler>();

            // Repositories
            services.AddTransient<IRepository<Ehr>, EhrRepository>();
            services.AddTransient<IRepository<VersionedEhrStatus>, VersionedEhrStatusRepository>();
            services.AddTransient<IRepository<Composition>, CompositionRepository>();
            services.AddTransient<IRepository<VersionedComposition>, VersionedCompositionRepository>();

            // HttpClient
            services.AddHttpClient<IEhrClient, EhrClient>((services, client) =>
            {
                var config = services.GetRequiredService<IConfiguration>();
                var httpClientConfig = config.GetSection("HttpClients");
                var baseUrl = httpClientConfig.GetValue("EhrClient:BaseUrl", string.Empty);
                var timeout = httpClientConfig.GetValue("EhrClient:Timeout", 30);
                var preferType = httpClientConfig.GetValue("EhrClient:PreferType", "minimal");
                var acceptType = httpClientConfig.GetValue("EhrClient:AcceptType", "application/json");

                client.DefaultRequestHeaders.Add("User-Agent", "Shellscripts.OpenEhr");
                client.DefaultRequestHeaders.Add("Prefer", preferType);
                client.DefaultRequestHeaders.Add("Accept", acceptType);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.BaseAddress = new Uri(baseUrl);
            }).AddHttpMessageHandler<EhrClientUrlHandler>();

            // JsonConverters
            services.AddSingleton<UidConverter>();
            services.AddSingleton<ObjectIdConverter>(); 
            services.AddSingleton<ObjectRefConverter>();
            services.AddSingleton<DataValueConverter>();
            services.AddSingleton<PartyProxyConverter>();
            services.AddSingleton<PathableConverter>();

            services.AddSingleton<EhrConverter>();
            services.AddSingleton<VersionedObjectConverter>();

            // Non-Standard
            services.AddSingleton<ResultSetRowConverter>();

            services.AddSingleton(provider =>
            {
                var options = new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,                    
                    WriteIndented = true,
                    IgnoreReadOnlyFields = true,
                    IgnoreReadOnlyProperties = true,
                    PropertyNameCaseInsensitive = true                    
                };

                // JsonConverters
                options.Converters.Add(provider.GetRequiredService<UidConverter>());
                options.Converters.Add(provider.GetRequiredService<ObjectIdConverter>());
                options.Converters.Add(provider.GetRequiredService<ObjectRefConverter>());
                options.Converters.Add(provider.GetRequiredService<DataValueConverter>());
                options.Converters.Add(provider.GetRequiredService<PartyProxyConverter>());
                options.Converters.Add(provider.GetRequiredService<PathableConverter>());

                options.Converters.Add(provider.GetRequiredService<EhrConverter>());
                options.Converters.Add(provider.GetRequiredService<VersionedObjectConverter>());

                // Non Standard
                options.Converters.Add(provider.GetRequiredService<ResultSetRowConverter>());

                return options;
            });

            services.AddSingleton<ITypeMapLookup>(provider => new TypeMapLookup(Assembly.GetExecutingAssembly()));
        }
    }
}
