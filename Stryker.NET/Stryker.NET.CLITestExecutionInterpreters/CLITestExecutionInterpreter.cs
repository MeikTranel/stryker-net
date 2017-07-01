using System;
using System.IO;

namespace Stryker.NET.CLITestExecutionInterpreters
{
    public abstract class CLITestExecutionInterpreter
    {
        private Stream _StreamToInterpret;

        public CLITestExecutionInterpreter(Stream StreamToInterpret)
        {
            _StreamToInterpret = StreamToInterpret;
        }

        public event EventHandler<TestCaseEventArgs> UnitTestExecutionFinished;

        protected virtual void OnUnitTestExecutionFinished(TestCaseEventArgs e)
        {
            UnitTestExecutionFinished?.Invoke(this, e);
        }
    }
}