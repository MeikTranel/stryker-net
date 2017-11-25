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
            var rootFolder = ".."; //Directory.GetCurrentDirectory();
            var reporter = new CleartTextReporter();
            var directoryManager = new DirectoryManager(reporter);
            using (var stryker = new Stryker(directoryManager, reporter, rootFolder))
            {
                try {
                    stryker.RunMutationTests();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }                     
            
            Console.ReadKey();
        }
    }
}