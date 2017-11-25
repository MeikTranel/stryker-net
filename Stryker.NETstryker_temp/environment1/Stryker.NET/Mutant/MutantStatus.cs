using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET
{
    public enum MutantStatus
    {
        NoCoverage,
        Killed,
        Survived,
        TimedOut,
        RuntimeError
    }
}
