namespace Shellscripts.OpenEHR.TestConsole
{
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Shellscripts.OpenEHR.Configuration;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Repositories;
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
                                await LoadDataFromEHRSandboxExample(host);
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

        private static async Task LoadDataFromEHRSandboxExample(IHost host)
        {
            // Get the Repositories to load the data
            var ehrRepo = host.Services.GetRequiredService<IRepository<Ehr>>();
            var compRepo = host.Services.GetRequiredService<IRepository<Composition>>();
            var token = CancellationToken.None;

            var ehrId = "eecf24e0-5ac9-4bfc-b958-475162940444";
            var comp1Id = "cdc46572-9074-451e-aeee-843ae2e44ecd";
            var comp2Id = "cdc46572-9074-451e-aeee-843ae2e44ecd::local.ehrbase.org::1";

            var ehrParams = new Dictionary<string, string>{ {"ehrId", ehrId } };
            var comp1Params = new Dictionary<string, string> { { "ehrId", ehrId }, { "compositionId", comp1Id } };
            var comp2Params = new Dictionary<string, string> { { "ehrId", ehrId }, { "compositionId", comp2Id } };

            var ehr = await ehrRepo.GetSingleAsync(ehrParams, token);
            var composition_object_1 = await compRepo.GetSingleAsync(comp1Params, token);
            var composition_object_2 = await compRepo.GetSingleAsync(comp2Params, token);

            ;
        }

        #endregion



        
    }
}
