﻿using System;
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
            var reporter = new CleartTextReporter();
            var directoryManager = new DirectoryManager(reporter);
            using (var stryker = new Stryker(runner, directoryManager, reporter, rootFolder))
            {
                stryker.PrepareEnvironment();
                stryker.RunMutationTest();
            }              
            
            Console.ReadKey();
        }
    }
}