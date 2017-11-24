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
            var rootFolder = @"C:\Repos\stryker-net-new\stryker-net\Stryker.NET"; 
            var runner = new TestRunner(rootFolder);
            var directoryManager = new DirectoryManager();
            var reporter = new CleartTextReporter(rootFolder);
            var stryker = new Stryker(runner, directoryManager, reporter, rootFolder);
            stryker.PrepareEnvironment();
            stryker.RunMutationTest();
            
            Console.ReadKey();
        }
    }
}