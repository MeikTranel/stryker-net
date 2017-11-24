using System.Collections.Generic;
using System.IO;
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

                //For future refrence: http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn
                var codeToWrite = mutant.MutatedCode;
                
                /* (over)write temp code file
                using (var streamWriter = new FileStream(mutant.FilePath, FileMode.Create))
                {
                    // write text
                }*/

                // restore mutant to original state
                //string restoredCode = mutatorOrchestrator.Restore(mutant);
            }
        }
    }
}