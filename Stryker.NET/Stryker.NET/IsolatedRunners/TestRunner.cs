using System;
using System.Diagnostics;
using Stryker.NET.Managers;

namespace Stryker.NET.IsolatedRunner
{
    public class TestRunner : ITestRunner
    {
        private readonly string _rootDirectory;
        private readonly string _testDir;
        private readonly string _command;

        public TestRunner(string testDir)
        {
            _testDir = testDir;
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
                WorkingDirectory = _testDir,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true
            };
            var process = Process.Start(info);
            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
