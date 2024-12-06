using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Shellscripts.OpenEHR.Tests.Context
{
    public class TestContext : IDisposable
    {



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseTest
    {
        internal ITestOutputHelper OutputHelper { get; }

        public BaseTest(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
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
