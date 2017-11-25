using System;
using System.Collections.Generic;
using System.Text;
using Stryker.NET.Core.Event;

namespace Stryker.NET.Reporters
{
    public interface IReporter : IFileReadHandler, IMutantTestedHandler, IScoreHandler, IWrapUpHandler, ITestMatchHandler, IDisposable
    {
    }
}
