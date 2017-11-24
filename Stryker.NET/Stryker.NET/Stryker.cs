using System.Collections.Generic;
using Stryker.NET.Managers;

namespace Stryker.NET
{
    class Stryker
    {
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
            _directoryManager.CreateDirectory($"{_rootdir}\\Temp");
        }

        public void RunMutationTest()
        {
            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(Files);

            foreach (var mutant in mutants)
            {
                System.Console.WriteLine($"Mutated '{mutant.OriginalFragment}' to '{mutant.MutatedFragment}' using mutator {mutant.MutatorName}");


            }
        }
    }
}