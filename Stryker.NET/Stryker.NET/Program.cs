using System;
using System.IO;

namespace Stryker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            string[] files = { Path.Combine(rootFolder, "Math.cs") };
            var stryker = new Stryker(files);
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}