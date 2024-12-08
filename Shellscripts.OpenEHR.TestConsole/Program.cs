namespace Shellscripts.OpenEHR.TestConsole
{
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Shellscripts.OpenEHR.Configuration;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;
    

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
                ConfigureAppConfiguration((context, builder) => ContainerConfiguration.ConfigureAppConfiguration(context, builder, args)).
                ConfigureLogging(ContainerConfiguration.ConfigureLogging).
                ConfigureServices(ContainerConfiguration.ConfigureServices).
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



        
    }
}
