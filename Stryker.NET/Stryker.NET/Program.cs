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
            string[] files = { Path.Combine(rootFolder, "Math.cs") };
            var runner = new TestRunner(rootFolder);
            var directoryManager = new DirectoryManager();
            var reporter = new CleartTextReporter(rootFolder);
            var stryker = new Stryker(runner, directoryManager, reporter, files, rootFolder);
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}