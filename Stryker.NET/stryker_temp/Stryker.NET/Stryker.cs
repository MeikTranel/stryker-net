using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;

namespace Stryker.NET
{
    class Stryker : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _mutationDir;
        private readonly IDirectoryManager _directoryManager;
        private string _rootdir;
        private readonly ITestRunner _testRunner;
        public IEnumerable<string> _files { get; private set; }

        public Stryker(ITestRunner testRunner, 
            IDirectoryManager directoryManager, 
            string rootdir)
        {
            _testRunner = testRunner;
            _directoryManager = directoryManager;
            _rootdir = rootdir;
            _tempDir = $"{rootdir}\\stryker_temp";
            _mutationDir = $"{_tempDir}\\Stryker.NET";
        }

        public void PrepareEnvironment()
        {
            _directoryManager.CopyRoot(_rootdir, _tempDir);
        }

        public void RunMutationTest()
        {
            if (_testRunner == null)
            {
                throw new Exception("A test runner was not specified");
            }
            _files = _directoryManager.GetFiles(_mutationDir);

            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(_files);

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