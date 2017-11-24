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
            _tempDirectory = $"{_rootDirectory}\\stryker_temp";
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
                WorkingDirectory = _tempDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info);
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //ignore
            //System.Console.WriteLine(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //ignore
            //System.Console.WriteLine(e.Data);
        }
    }
}
