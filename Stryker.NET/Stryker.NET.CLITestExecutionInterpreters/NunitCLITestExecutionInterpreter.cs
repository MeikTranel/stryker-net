using System.IO;

namespace Stryker.NET.CLITestExecutionInterpreters
{
    public class NunitCLITestExecutionInterpreter : CLITestExecutionInterpreter
    {
        public NunitCLITestExecutionInterpreter(Stream StreamToInterpret) : base(StreamToInterpret)
        {
        }
    }
}