using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Reporters
{
    interface IReporter : IDisposable
    {
        void OnSourceFileRead();
        void OnAllSourceFilesRead();
        void OnAllMutantsMatchedWithTests();
        void OnMutantTested(MutantResult result);
        void OnAllMutantsTested(IReadOnlyCollection<MutantResult> results);
        void OnScoreCalculated();
        void OnWrapUp();
    }
}
