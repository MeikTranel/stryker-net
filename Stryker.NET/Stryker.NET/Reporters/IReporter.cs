using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Reporters
{
    public interface IReporter : IDisposable
    {
        void Report(Mutant mutant);
    }
}
