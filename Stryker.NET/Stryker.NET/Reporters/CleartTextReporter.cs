using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Reporters
{
    public class CleartTextReporter : IReporter
    {        
        /// <summary>
        /// Report the status and changes of the passed mutant to the console.
        /// 
        /// Example report:
        /// 
        /// Mutant killed!
        /// /yourPath/yourFile.js: line 10:27
        /// Mutator: BinaryOperator
        /// -                 return user.age >= 18;
        /// +                 return user.age > 18;
        /// </summary>
        /// <param name="mutant">The mutant to report</param>
        public void Report(Mutant mutant)
        {
            // Create a new stringbuilder instance for efficient report creation
            var sb = new StringBuilder();

            // Write report data to the stringbuilder
            sb.AppendLine($"Mutant status unknown");
            sb.AppendLine($"{mutant.FilePath}: line {mutant.LinePosition.Line + 1}:{mutant.LinePosition.Character + 1}"); //LinePosition starts at 0, not 1
            sb.AppendLine($"Mutator: {mutant.MutatorName}");
            sb.AppendLine($"-\t\t{mutant.OriginalFragment}");
            sb.AppendLine($"+\t\t{mutant.MutatedFragment}");

            // write to output (console)
            Console.WriteLine(sb.ToString());
        }

        public void Dispose()
        {
            
        }
    }
}
