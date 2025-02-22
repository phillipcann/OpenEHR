# Shellscripts.OpenEHR

## Contents
<!--TOC-->
  - [Description](#description)
  - [Implementation](#implementation)
    - [Reference Model](#reference-model)
    - [Configuration](#configuration)
    - [Library](#library)
  - [Testing](#testing)
  - [Appendix](#appendix)
    - [Examples](#examples)
      - [01 - Load some records from the Open EHR Sandbox](#01-load-some-records-from-the-open-ehr-sandbox)
  - [License](#license)
<!--/TOC-->

## Description
[<sup><sub>Home</sub></sup>](#contents)

The `Shellscripts.OpenEHR` library has been constructed to assist with the integration to Open EHR servers from within C# code. When I first started working with OpenEHR, I was not able to find much in the way of integration using the C# language where there was a need to use Complex Mapping from MVC or Web API View Models. Integration had to be done with JSON / Javascript and it was a bit of an arduous process to load content from OpenEHR and map to and from View Models. 

The library hopes to address the problems of 
- Being able to load Json from an Open EHR server into strongly typed objects for mapping to application Models
- Being able to map application Models to strongly typed Reference Model items for easy Serialisation to Json and persistance to an Open EHR Server


<p style="background-color: #FF7777; color: #000000;">
This library is very much in the experimental stages. The serialisation / deserialisation routines are being actively worked on but are quite involved and taking some quite time to piece together. Part of the reason I suspect there didn't seem to be anything around that already does this. There are plenty of unit tests in the solution to start testing the different use cases.
</p>

## Implementation
[<sup><sub>Home</sub></sup>](#contents)
### Reference Model 
The solution contains most of the classes from the following Open EHR Reference Models as Objects that can be Serialised and Deserialised to and from. With more iterations of the solution, more classes get implemented as I find classes I've missed out or had not implemented so as to get a working solution checked in and passing tests.


| Reference Model Item |
| -- | 
| [Base Types](https://specifications.openehr.org/releases/BASE/latest/base_types.html#_base_types) |
| [Common Information Model](https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_common_information_model) | 
| [Data Structures](https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_data_structures_information_model) | 
| [Data Types](https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_data_types_information_model) | 
| [EHR Information Model](https://specifications.openehr.org/releases/RM/latest/ehr.html#_ehr_information_model) | 
| [Foundation Types](https://specifications.openehr.org/releases/BASE/development/foundation_types.html) |
| [Platform Service Model](https://specifications.openehr.org/releases/SM/latest/openehr_platform.html) |

### Configuration
[<sup><sub>Home</sub></sup>](#contents)

In the `appsettings.Development.json` file, the following configuration is in place at the moment.

```json
    {
        "HttpClients": {
            "EhrClient": {
                // "Login": "https://sandkiste.ehrbase.org/",

                "BaseUrl": "https://walrus-app-73l8x.ondigitalocean.app",
                "SystemUri": "/ehrbase-ehrbase/ehrbase/rest/openehr/v1", // append to BaseUrl

                "Timeout": 120,
                "ReturnType": "representation", // minimal, representation
                "AcceptType": "application/json" // application/json, application/xml, text/plain, application/openehr.wt+json
            }
        }
    }
```

### Library
[<sup><sub>Home</sub></sup>](#contents)

The solution has been constructed using the .NET 8 framework and makes use of the `Microsoft.Extensions` packages for Dependency Resolution and Inversion of Control.

A setup class has been constructed which will load application configuration files, setup logging and register application dependencies.

`Shellscripts.OpenEHR.Configuration.ContainerConfiguration` is a static class with the following static methods available.

```C#
    public static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args) 
    { ... }

    public static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
    { ... }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    { ... }
```

The built in configuration methods will register the Client, Handler, Repositories and Serialisers that all used together can pull data from an Open EHR server and map the Json results to strongly typed objects. The custom serialiser classes will allow for turning those strongly typed objects back into the appropriate format Json to submit to the Open EHR server for persistenting.


## Testing
[<sup><sub>Home</sub></sup>](#contents)

There is a number of unit tests (Xunit) in the project in the `Shellscripts.OpenEHR.Tests` project. There is also a Console application `Shellscripts.OpenEHR.TestConsole` which is setup as the default startup project.

In the Unit Tests project, the TestFixture makes use of the Configuration mentioned above to ensure the same dependencies used by the library are available for testing within the Unit Test project

See also [Testing Readme](./Shellscripts.OpenEHR.Tests/README.md)

```C#
    /// <summary>
    /// TestFixture sets up the Configuration and Service registration that's used in the main application
    /// </summary>
    public class TestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public TestFixture()
        {
            // Host
            var hostProperties = new Dictionary<object, object>();
            var environment = new HostingEnvironment() { ContentRootPath = Directory.GetCurrentDirectory(), EnvironmentName = "Development" };
            HostBuilderContext context = new HostBuilderContext(properties: hostProperties) { HostingEnvironment = environment };

            // Configuration
            var testConfigurationBuilder = new ConfigurationBuilder();
            ContainerConfiguration.ConfigureAppConfiguration(context, testConfigurationBuilder, args: Array.Empty<string>());
            Configuration = testConfigurationBuilder.Build();

            // Services
            var testServiceCollection = new ServiceCollection();
            testServiceCollection.AddSingleton(Configuration);            
            ContainerConfiguration.ConfigureServices(context, testServiceCollection);

            // Logging            
            testServiceCollection.AddLogging((lb) => {
                lb.ClearProviders();
                lb.AddDebug();
                lb.AddProvider(new TestOutputLoggerProvider(this.OutputHelper));
            });

            ServiceProvider = testServiceCollection.BuildServiceProvider();
        }

        public void Dispose() { ... }
    }
```

## Appendix
[<sup><sub>Home</sub></sup>](#contents)

### Examples

#### 01 - Load some records from the Open EHR Sandbox

```C#
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
```

## License
[<sup><sub>Home</sub></sup>](#contents)

This Library is released under the [MIT License](LICENSE)