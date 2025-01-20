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
        private static ILogger? Logger { get; set; }
        private static JsonSerializerOptions? SerialiserOptions { get; set; }

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

            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Program");
            SerialiserOptions = host.Services.GetRequiredService<JsonSerializerOptions>();

            await host.StartAsync();

            #endregion

            #region Main Loop

            var continueHostRun = true;
            await ShowMenu();

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

                            case ConsoleKey.D2:
                                await BuildEhrRecordExample(host);
                                break;

                            case ConsoleKey.Q:
                                continueHostRun = false;
                                break;                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, ex.Message);
                    }

                    await ShowMenu();
                }
            }

            #endregion

            #region Close and Dispose

            await host.StopAsync();

            #endregion
        }

        #endregion

        #region Loop Methods

        private static async Task BuildEhrRecordExample(IHost host)
        {
            // Allow us to debug...
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(600));

            // 1. Get the Repos
            var ehrRepo = host.Services.GetRequiredService<IRepository<Ehr>>();
            var compRepo = host.Services.GetRequiredService<IRepository<Composition>>();

            // 2. Check if we've got a test Ehr
            Logger?.LogInformation($"Checking for Ehr Id");
            var ehr_params = new Dictionary<string, string>() { { "subject_id", "8888812345" }, { "subject_namespace", "https://fhir.nhs.uk/nhsnumber" } };
            var ehr = await ehrRepo.GetSingleAsync( ehr_params, tokenSource.Token);
            Logger?.LogInformation($"Ehr Id\t: {ehr}");

            // 2.1. If Ehr Is Empty, build and create one
            if (string.IsNullOrWhiteSpace(ehr?.EhrId.Value))
            {
                ehr = new Ehr()
                {
                    EhrStatus = new EhrStatus()
                    {
                        ArchetypeNodeId = "openEHR-EHR-EHR_STATUS.generic.v1",
                        Name = new Models.DataTypes.DvText()
                        {
                            Value = "Joseph Bloggs"
                        },
                        Subject = new Models.CommonInformation.PartySelf()
                        {
                            ExternalReference = new Models.BaseTypes.PartyRef()
                            {
                                Namespace = ehr_params["subject_namespace"],
                                Id = new Models.BaseTypes.ObjectId()
                                {
                                    Value = ehr_params["subject_id"]
                                }
                            }
                        },
                        IsModifiable = true
                    }
                };

                //// What does the Json look like?
                //var ehr_json = JsonSerializer.Serialize(ehr, SerialiserOptions);
                //Logger?.LogInformation(ehr_json);

                Logger?.LogInformation("Upserting Ehr");
                var ehr_id = await ehrRepo.UpsertAsync(ehr, tokenSource.Token);
                Logger?.LogInformation($"Ehr Id\t: {ehr_id}");
            }

            Console.WriteLine("Press Enter to return to Menu");
            Console.ReadLine();
        }

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
        }

        private static async Task ShowMenu()
        {
            await Task.Run(() =>
            {
                Console.Clear();
                Console.WriteLine($"\t\t1\tLoad Data From EHR Sandbox Example");
                Console.WriteLine($"\t\t2\tBuild Ehr Record Example");
                Console.WriteLine();
                Console.WriteLine($"\t\tQ\tQuit");
            });
        }

        #endregion



        
    }
}
