using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Stryker.NET
{
    public class TestRunner : ITestRunner
    {
        public void Test(string rootDir)
        {
            var info = new ProcessStartInfo()
            {
                UseShellExecute = false,
                WorkingDirectory = rootDir,
                Arguments = "test",
                FileName = "dotnet"
            };
            Process.Start(info);
        }
    }
}
