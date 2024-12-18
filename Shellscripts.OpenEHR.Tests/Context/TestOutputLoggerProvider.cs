namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    public class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestOutputLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName) => new TestOutputLogger(_testOutputHelper, categoryName);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private class TestOutputLogger : ILogger
        {
            private readonly ITestOutputHelper _testOutputHelper;
            private readonly string _categoryName;

            public TestOutputLogger(ITestOutputHelper testOutputHelper, string categoryName)
            {
                _testOutputHelper = testOutputHelper;
                _categoryName = categoryName;
            }

            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (formatter != null)
                {
                    try
                    {
                        _testOutputHelper.WriteLine($"[{logLevel}] {_categoryName}: {formatter(state, exception)}");
                    }
                    catch (InvalidOperationException)
                    {
                        // Swallow exceptions that occur if the test output is no longer available.
                    }
                }
            }
        }
    }
}
