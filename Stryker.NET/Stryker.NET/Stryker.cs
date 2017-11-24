using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;

namespace Stryker.NET
{
    class Stryker
    {
        private readonly string tempDirName = ".stryker_temp";

        private readonly IDirectoryManager _directoryManager;
        private string _rootdir;
        private readonly ITestRunner _testRunner;
        public IEnumerable<string> Files { get; private set; }

        public Stryker(ITestRunner testRunner, 
            IDirectoryManager directoryManger, 
            IEnumerable<string> files,
            string rootdir)
        {
            Files = files;
            _testRunner = testRunner;
            _directoryManager = directoryManger;
            _rootdir = rootdir;
        }

        public void PrepareEnvironment()
        {
            _directoryManager.CreateDirectory($"{_rootdir}\\{tempDirName}");
        }

        public void RunMutationTest()
        {
            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(Files);

            foreach (var mutant in mutants)
            {
                System.Console.WriteLine($"Mutated '{mutant.OriginalFragment}' to '{mutant.MutatedFragment}' using mutator {mutant.MutatorName}");

                var path = mutant.FilePath;

                // (over)write temp code file
                File.WriteAllText(path, mutant.MutatedCode);

                // run unit tests with mutant
                

                // restore mutant to original state
                string restoredCode = mutatorOrchestrator.Restore(mutant);
                File.WriteAllText(path, restoredCode);
            }
        }
    }
}