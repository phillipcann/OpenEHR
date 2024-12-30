namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using System.Collections.Generic;
    using Shellscripts.OpenEHR.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;    
    using Microsoft.Extensions.Hosting.Internal;
    using Microsoft.Extensions.Logging;
    using Xunit;    
    using Xunit.Abstractions;

    /// <summary>
    /// TestFixture sets up the Configuration and Service registration that's used in the main application
    /// </summary>
    public class TestFixture : IDisposable
    {
        public IServiceProvider? ServiceProvider { get; private set; }
        public IConfiguration? Configuration { get; private set; }
        public ITestOutputHelper? OutputHelper { get; private set; }

        public TestFixture() { }

        public void SetOutputHelper(ITestOutputHelper helper)
        {
            OutputHelper = helper;
            SetupDependencyResolution();
        }

        public void Dispose()
        {
            if (ServiceProvider is IDisposable disposableSp)
            {
                disposableSp.Dispose();
            }
        }

        private void SetupDependencyResolution()
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
                lb.AddProvider(new TestOutputLoggerProvider(this.OutputHelper));
            });

            ServiceProvider = testServiceCollection.BuildServiceProvider();
        }
    }

    [CollectionDefinition("TestCollection")]
    public class TestCollection : ICollectionFixture<TestFixture> { }
}
