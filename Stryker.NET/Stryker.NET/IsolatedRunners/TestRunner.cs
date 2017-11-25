using System;
using System.Diagnostics;
using Stryker.NET.Managers;

namespace Stryker.NET.IsolatedRunner
{
    public class TestRunner : ITestRunner
    {
        private readonly string _testDir;
        private readonly string _command;

        public TestRunner(string testDir)
        {
            _testDir = testDir;
            _command = "dotnet";
        }

        public bool Test()
        {
            var arguments = $"test";
            var exitcode = RunCommand(arguments);
            if (exitcode == 1)
            {
                return true;
            }
            return false;
        }

        private int RunCommand(string arguments)
        {
            var info = new ProcessStartInfo(_command, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = _testDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
