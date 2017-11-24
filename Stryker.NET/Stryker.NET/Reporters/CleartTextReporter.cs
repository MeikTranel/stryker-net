using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Reporters
{
    public class CleartTextReporter : IReporter
    {
        public string OutputDir { get; private set; }

        public CleartTextReporter(string outputDir)
        {
            OutputDir = outputDir;
        }

        /* 
          Mutant killed!
          /yourPath/yourFile.js: line 10:27
          Mutator: BinaryOperator
          -                 return user.age >= 18;
          +                 return user.age > 18;

          Mutant survived!
          /yourPath/yourFile.js: line 10:27
          Mutator: RemoveConditionals
          -                 return user.age >= 18;
          +                 return true;
        */
        public void Report(Mutant mutant)
        {
            Console.WriteLine($"Mutant status unknown");
            Console.WriteLine($"{mutant.FilePath}: line {mutant.LinePosition.Line + 1}:{mutant.LinePosition.Character + 1}"); //LinePosition starts at 0, not 1
            Console.WriteLine($"Mutator: {mutant.MutatorName}");
            Console.WriteLine($"-\t\t{mutant.OriginalFragment}");
            Console.WriteLine($"+\t\t{mutant.MutatedFragment}");
            Console.WriteLine();
        }

        public void Dispose()
        {
            
        }
    }
}
