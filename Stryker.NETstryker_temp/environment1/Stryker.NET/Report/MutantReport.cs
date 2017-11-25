using System;
using System.Collections.Generic;
using System.Text;
using Stryker.NET.Core;

namespace Stryker.NET.Reporters
{
    public class MutantResult
    {
        public string SourceFilePath { get; set; }
        public string MutatorName { get; set; }
        public MutantStatus Status { get; set; }
        public string Replacement { get; set; }
        public string OriginalLines { get; set; }
        public string MutatedLines { get; set; }
        public IEnumerable<string> TestsRan { get; set; }
        public Location Location { get; set; }
        public Range<int> Range { get; set; }

        public MutantResult(
            string sourceFilePath, 
            string mutatorName, 
            MutantStatus status, 
            string replacement, 
            string originalLines, 
            string mutatedLines, 
            IEnumerable<string> testsRan, 
            Location location, 
            Range<int> range)
        {
            SourceFilePath = sourceFilePath;
            MutatorName = mutatorName;
            Status = status;
            Replacement = replacement;
            OriginalLines = originalLines;
            MutatedLines = mutatedLines;
            TestsRan = testsRan;
            Location = location;
            Range = range;
        }
    }
}
