using System;
using System.IO;
using Stryker.NET.IsolatedRunner;
using Microsoft.Extensions.Configuration;
using Stryker.NET.Managers;
using Stryker.NET.Reporters;

namespace Stryker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootFolder = Directory.GetCurrentDirectory(); 
            var runner = new TestRunner(rootFolder);
            var directoryManager = new DirectoryManager();
            var reporter = new CleartTextReporter(rootFolder);
            using (var stryker = new Stryker(runner, directoryManager, reporter, rootFolder))
            {
                stryker.PrepareEnvironment();
                stryker.RunMutationTest();
            }              
            
            Console.ReadKey();
        }
    }
}