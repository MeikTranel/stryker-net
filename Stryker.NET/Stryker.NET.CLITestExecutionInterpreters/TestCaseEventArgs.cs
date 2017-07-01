using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.CLITestExecutionInterpreters
{
    public class TestCaseEventArgs : EventArgs
    {
        public string TestCaseName { get; internal set; }
    }
}