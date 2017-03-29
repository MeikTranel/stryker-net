using System.Collections.Generic;

namespace Stryker.NET
{
    class Stryker
    {
        public IEnumerable<string> Files { get; private set; }

        public Stryker(IEnumerable<string> files)
        {
            Files = files;
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
