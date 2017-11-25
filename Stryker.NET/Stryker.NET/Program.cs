using System;
using System.Diagnostics;
using System.IO;
using Stryker.NET.IsolatedRunner;
using Stryker.NET.Managers;
using Stryker.NET.Reporters;

namespace Stryker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: get from args or a appconfig
            var timer = new Stopwatch();
            timer.Start();
            var rootFolder = @".."; 
            var directoryManager = new DirectoryManager();
            var reporter = new CleartTextReporter();
            using (var stryker = new Stryker(directoryManager, reporter, rootFolder))
            {
                stryker.RunMutationTests();
            }
            timer.Stop();
            Console.WriteLine($"Elapsed timeL {timer.Elapsed}");
            Console.ReadKey();
        }
    }
}