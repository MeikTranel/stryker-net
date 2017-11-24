using System.Diagnostics;
using Stryker.NET.Managers;

namespace Stryker.NET.IsolatedRunner
{
    public class TestRunner : ITestRunner
    {
        private readonly string _rootDirectory;        
        private readonly string _tempDirectory;
        private readonly string _command;

        public TestRunner(string rootDir)
        {
            _rootDirectory = rootDir;
            _tempDirectory = $"{_rootDirectory}\\Temp";
            _command = "dotnet";
            
        }

        public void Test()
        {
            var arguments = $"test";
            RunCommand(arguments);

        }

        private void RunCommand(string arguments)
        {
            var info = new ProcessStartInfo(_command, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = _tempDirectory
            };
            var process = Process.Start(info);
            process.WaitForExit();
        }
    }
}
