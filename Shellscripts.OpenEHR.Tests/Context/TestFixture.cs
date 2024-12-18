namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using System.Collections.Generic;
    using Shellscripts.OpenEHR.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;    
    using Microsoft.Extensions.Hosting.Internal;
    using Xunit;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;
    using Xunit.Sdk;

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


        public void Dispose()
        {
            if (ServiceProvider is IDisposable disposableSp)
            {
                disposableSp.Dispose();
            }

            if (Configuration is IDisposable disposableCfg)
            {
                disposableCfg.Dispose();
            }
        }
    }

    [CollectionDefinition("TestCollection")]
    public class TestCollection : ICollectionFixture<TestFixture> { }
}
