namespace Shellscripts.OpenEHR.TestConsole
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;
    using Shellscripts.OpenEHR.Serialisation.Converters;

    internal class Program
    {
        private static ILogger? logger { get; set; }

        #region Main / MainAsync

        static void Main(string[] args)
        {
            Task.Run(() => MainAsync(args)).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            #region Host Setup

            var host = Host.CreateDefaultBuilder(args).
                ConfigureAppConfiguration((context, builder) => ConfigureAppConfiguration(context, builder, args)).
                ConfigureLogging(ConfigureLogging).
                ConfigureServices(ConfigureServices).
                Build();

            logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Program");

            await host.StartAsync();

            #endregion

            #region Main Loop

            var continueHostRun = true;
            while (continueHostRun)
            {
                if (Console.KeyAvailable)
                {
                    var cKey = Console.ReadKey(intercept: true).Key;
                    
                    try 
                    {                     
                        switch (cKey)
                        {
                            case ConsoleKey.D1:
                                await OptionOne(host);
                                break;

                            case ConsoleKey.Q:
                                continueHostRun = false;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, ex.Message);
                    }
                }
            }

            #endregion

            #region Close and Dispose

            await host.StopAsync();

            #endregion
        }

        #endregion

        #region Loop Methods

        private static async Task OptionOne(IHost host)
        {
            // Get the Client Factory
            var client = host.Services.GetRequiredService<IEhrClient>();
            var options = host.Services.GetRequiredService<JsonSerializerOptions>();

            // Test the ExecuteAsync Method
            var results = await client.ExecuteAsync(async (c, ct) =>
            {
                var rm = await c.GetAsync("/ehr/eecf24e0-5ac9-4bfc-b958-475162940444/versioned_ehr_status");

                rm.EnsureSuccessStatusCode();

                var stringContent = await rm.Content.ReadAsStringAsync();

                var deserialisedData = JsonSerializer.Deserialize<VersionedEhrStatus>(stringContent, options);

                return deserialisedData;
            });
        }

        #endregion


        #region Setup

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            string env = context.HostingEnvironment.EnvironmentName;

            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.ClearProviders();

            if (context.HostingEnvironment.IsDevelopment())
            {
                builder
                    .AddConsole()
                    .AddDebug();
            }
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
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

                var baseUrl = new Uri(httpClientConfig.GetValue("EhrClient1:BaseUrl", string.Empty));
                var timeout = httpClientConfig.GetValue("EhrClient1:Timeout", 30);
                var preferType = httpClientConfig.GetValue("EhrClient1:PreferType", "minimal");
                var acceptType = httpClientConfig.GetValue("EhrClient1:AcceptType", "application/xml");

                client.DefaultRequestHeaders.Add("User-Agent", "Shellscripts.OpenEhr");
                client.DefaultRequestHeaders.Add("Prefer", preferType);
                client.DefaultRequestHeaders.Add("Accept", acceptType);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.BaseAddress = baseUrl;
            });
                //.AddHttpMessageHandler<UriAppendingHandler>();

            // Singletons
            services.AddSingleton(provider =>
            {
                var options = new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                };
                
                options.Converters.Add(new DvDateTimeConverter());
                return options;
            });
        }

        #endregion
    }
}
