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

The `Shellscripts.OpenEHR` library has been constructed to assist with the integration to Open EHR servers from within C# code. There was found to be a gap in the tooling available for integration using the C# language where there was a need to use Complex Mapping from MVC or Web API View Models. Making it a slow process to adopt in the first instance.

The library hopes to address the problems of 
- Being able to load Json from an Open EHR server into strongly typed objects for mapping to application Models
- Being able to map application Models to strongly typed Reference Model items for easy Serialisation to Json and persistance to an Open EHR Server

## Implementation
[<sup><sub>Home</sub></sup>](#contents)
### Reference Model 
The solution contains all of the classes from the following Open EHR Reference Models as Objects that can be Serialised and Deserialised to and from.

| Reference Model Item |
| -- | 
| [Base Types](https://specifications.openehr.org/releases/BASE/latest/base_types.html#_base_types) |
| [Common Information Model](https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_common_information_model) | 
| [Data Structures](https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_data_structures_information_model)| 
| [Data Types](https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_data_types_information_model)| 
| [EHR Information Model](https://specifications.openehr.org/releases/RM/latest/ehr.html#_ehr_information_model)| 

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
            ContainerConfiguration.ConfigureServices(context, testServiceCollection);
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
        // Get the Client Factory
        var client = host.Services.GetRequiredService<IEhrClient>();
        var options = host.Services.GetRequiredService<JsonSerializerOptions>();
        var cancellationToken = CancellationToken.None;

        var ehrId = "eecf24e0-5ac9-4bfc-b958-475162940444";
        var compositionUid = "cdc46572-9074-451e-aeee-843ae2e44ecd::local.ehrbase.org::1";
        var compositionId = "cdc46572-9074-451e-aeee-843ae2e44ecd";

        Ehr ehr = await client.GetEhrAsync(ehrId, cancellationToken);

        Composition composition_object_1 = await client.GetCompositionAsync(ehrId, compositionId, cancellationToken);

        Composition composition_object_2 = await client.GetCompositionAsync(ehrId, compositionUid, cancellationToken);
    }
```

## License
[<sup><sub>Home</sub></sup>](#contents)

This Library is released under the [MIT License](LICENSE)