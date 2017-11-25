using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Reporters
{
    public class CleartTextReporter : IReporter
    {

        /// <summary>
        /// Eventhandler for 'MutantTested' event that report the status and changes of the passed mutantResult to the console.
        /// 
        /// Example report:
        /// 
        /// Mutant killed!
        /// /yourPath/yourFile.js: line 10:27
        /// Mutator: BinaryOperator
        /// -                 return user.age >= 18;
        /// +                 return user.age > 18;
        /// </summary>
        /// <param name="v">The mutantreport to display/write</param>
        public void OnMutantTested(MutantResult mutantResult)
        {
            // Create a new stringbuilder instance for efficient report creation
            var sb = new StringBuilder();

            // Write report data to the stringbuilder
            sb.AppendLine($"Mutant {mutantResult.Status}!");
            sb.AppendLine($"{mutantResult}: line {mutantResult.Location.Start.Line}:{mutantResult.Location.Start.Column}");
            sb.AppendLine($"Mutator: {mutantResult.MutatorName}");
            sb.AppendLine($"-\t\t{mutantResult.OriginalLines}");
            sb.AppendLine($"+\t\t{mutantResult.MutatedLines}");

            // write to output (console)
            Console.WriteLine(sb.ToString());
        }

        public void OnAllMutantsMatchedWithTests()
        {
            
        }

        public void OnAllMutantsTested(IReadOnlyCollection<MutantResult> results)
        {
            
        }

        public void OnScoreCalculated()
        {
            
        }

        public void OnWrapUp()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void OnSourceFileRead(string filePath)
        {
            
        }

        public void OnAllSourceFilesRead(string[] filePaths)
        {
            
        }
    }
}
