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
            var rootFolder = Directory.GetCurrentDirectory();
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