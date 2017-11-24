﻿using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;

namespace Stryker.NET
{
    class Stryker : IDisposable
    {
        private readonly string _tempDirName = ".stryker_temp";

        private readonly IDirectoryManager _directoryManager;
        private string _rootdir;
        private readonly ITestRunner _testRunner;
        public IEnumerable<string> Files { get; private set; }

        public Stryker(ITestRunner testRunner, 
            IDirectoryManager directoryManager, 
            IEnumerable<string> files,
            string rootdir)
        {
            Files = files;
            _testRunner = testRunner;
            _directoryManager = directoryManager;
            _rootdir = rootdir;
        }

        public void PrepareEnvironment()
        {
            _directoryManager.CreateDirectory($"{_rootdir}\\{_tempDirName}");
        }

        public void RunMutationTest()
        {
            if (_testRunner == null)
            {
                throw new Exception("A test runner was not specified");
            }

            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(Files);

            foreach (var mutant in mutants)
            {
                System.Console.WriteLine($"Mutated '{mutant.OriginalFragment}' to '{mutant.MutatedFragment}' using mutator {mutant.MutatorName}");

                var path = mutant.FilePath;

                // (over)write temp code file
                File.WriteAllText(path, mutant.MutatedCode, Encoding.Unicode);

                // run unit tests with mutant
                _testRunner.Test();

                // restore mutant to original state
                string restoredCode = mutatorOrchestrator.Restore(mutant);
                File.WriteAllText(path, restoredCode);
            }
        }

        public void Dispose()
        {
            _directoryManager.Dispose();
        }
    }
}