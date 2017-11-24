using System;
using System.IO;
using Stryker.NET.IsolatedRunner;

namespace Stryker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            string[] files = { Path.Combine(rootFolder, "Math.cs") };
            var stryker = new Stryker(files);
            var testRunner = new TestRunner(@"C:\Repos\stryker-net\Stryker.NET");
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}