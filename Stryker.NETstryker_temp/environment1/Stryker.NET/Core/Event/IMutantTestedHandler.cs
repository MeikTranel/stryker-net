using System.Collections.Generic;

namespace Stryker.NET.Reporters
{
    public delegate void MutantTestedDelegate(MutantResult result);
    public delegate void AllMutantsTestedDelegate(IReadOnlyCollection<MutantResult> result);

    public interface IMutantTestedHandler
    {
        void OnMutantTested(MutantResult result);
        void OnAllMutantsTested(IReadOnlyCollection<MutantResult> results);
    }
}