using System;
using System.IO;

namespace Stryker.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new StykerOptions();

            var reflector = new FileReflector(options, Directory.GetCurrentDirectory());
            string[] files = reflector.GetFilesToMutate();
            var stryker = new Stryker(files);
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}