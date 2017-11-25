using System;
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
            var rootFolder = @".."; 
            var runner = new TestRunner(rootFolder);
            var directoryManager = new DirectoryManager();
            var reporter = new CleartTextReporter();
            using (var stryker = new Stryker(directoryManager, reporter, rootFolder))
            {
                stryker.RunMutationTests();
            }              
            
            Console.ReadKey();
        }
    }
}