using System.Diagnostics;
using Stryker.NET.Managers;

namespace Stryker.NET.IsolatedRunner
{
    public class TestRunner : ITestRunner
    {
        private readonly string _baseDir;
        private readonly string _testDir;
        private readonly string _command;

        public TestRunner(string testDir, string baseDir)
        {
            _testDir = testDir;
            _baseDir = baseDir;
            _command = "dotnet";
        }

        public void Test()
        {
            RunCommand($"build --no-restore {_baseDir}\\{_testDir}\\Stryker.NET\\Stryker.NET.csproj");
            RunCommand($"test --no-build");
        }

        private void RunCommand(string arguments)
        {
            var info = new ProcessStartInfo(_command, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = $"{ _baseDir }\\{ _testDir }",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
