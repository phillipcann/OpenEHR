namespace Shellscripts.OpenEHR.TestConsole
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal class Program
    {
        private static ILogger? logger { get; set; }

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


            logger?.LogInformation("Started");
            while (true)
            {
                #region Main



                #endregion

                #region Main Breakout

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.Q)
                    {
                        logger?.LogInformation("Finished");
                        break;
                    }
                }

                #endregion
            }

          


            #region Close and Dispose

            await host.StopAsync();

            #endregion
        }

        #region Setup

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            string env = context.HostingEnvironment.EnvironmentName;

            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
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
            // Transients
            services.AddTransient<EhrClient>();

            
            
            // HttpClientFactory Clients
            services.AddHttpClient("EhrClientWithBasic", (sp, client) => 
            {
                var config = sp.GetRequiredService<IConfiguration>();

                client = sp.GetRequiredService<EhrClient>();
                client.BaseAddress = new Uri(config.GetSection("HttpClients").GetValue("EhrClient:BaseUrl", string.Empty));

                
            });
        }

        #endregion
    }
}
