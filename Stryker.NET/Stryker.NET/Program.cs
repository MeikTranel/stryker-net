using System;
using System.IO;
using Stryker.NET.IsolatedRunner;
using Stryker.NET.Managers;

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
            var stryker = new Stryker(runner, directoryManager, files, rootFolder);
            stryker.PrepareEnvironment();
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}