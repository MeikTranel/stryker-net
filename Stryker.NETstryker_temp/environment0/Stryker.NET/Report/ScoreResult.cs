using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Report
{
    public class ScoreResult
    {
        public int Killed { get; }
        public int TimedOut { get; }
        public int Survived { get; }
        public int NoCoverage { get; }

        public ScoreResult(int killed, int timedOut, int survived, int noCoverage)
        {
            Killed = killed;
            TimedOut = timedOut;
            Survived = survived;
            NoCoverage = noCoverage;
        }

        public int TotalDetected => Killed + TimedOut;
        public int TotalUndetected => Survived + NoCoverage;
        public int TotalInvalid => 0;
        public int TotalValid => TotalDetected + TotalUndetected;
        public int TotalMutants => TotalInvalid + TotalValid;
        public int TotalCovered => TotalDetected + Survived;
        public decimal MutationScore => (TotalDetected / TotalValid) * 100;
        public decimal MutationScoreBasedOnCoveredCode => (TotalDetected / TotalCovered) * 100;
    }
}
