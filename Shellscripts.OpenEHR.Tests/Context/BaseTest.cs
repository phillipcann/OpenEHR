namespace Shellscripts.OpenEHR.Tests.Context
{
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Xunit;
    using Xunit.Abstractions;


    [Collection("TestCollection")]
    public abstract class BaseTest
    {
        private readonly TestFixture _fixture;

        internal ITestOutputHelper OutputHelper { get; }

        internal IConfiguration Configuration => _fixture.Configuration;
        internal IServiceProvider Services => _fixture.ServiceProvider;

        public BaseTest(ITestOutputHelper outputHelper, TestFixture testFixture)
        {
            this.OutputHelper = outputHelper;
            this._fixture = testFixture;
        }

        internal static async Task<string> LoadAssetAsync(string assetFile)
        {
            if (string.IsNullOrWhiteSpace(assetFile))
                throw new FileNotFoundException($"{nameof(assetFile)} cannot be NULL or Empty");

            string assetsDirectory = "Assets";

            var baseDirectory = Directory.GetCurrentDirectory(); // likely to be the bin directory?

            while (baseDirectory != null && !Directory.GetDirectories(baseDirectory, assetsDirectory).Any())
                baseDirectory = Directory.GetParent(baseDirectory)?.FullName;

            if (baseDirectory == null)
                throw new InvalidOperationException("NULL Base Directory");

            baseDirectory = Path.Combine(baseDirectory, assetsDirectory);
            if (!Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException(baseDirectory);

            var fullPath = baseDirectory;
            if (assetFile.Contains('/'))
            {
                foreach (var pathPart in assetFile.Split("/"))
                {
                    fullPath = Path.Combine(fullPath, pathPart);
                }
            }
            else
            {
                fullPath = Path.Combine(fullPath, assetFile);
            }

            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            return await File.ReadAllTextAsync(fullPath, Encoding.UTF8);
        }
    }
}
