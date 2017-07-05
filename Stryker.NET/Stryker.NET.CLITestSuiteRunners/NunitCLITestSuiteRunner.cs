using System;
using System.Diagnostics;

namespace Stryker.NET.CLITestSuiteRunners
{
    public class NunitCLITestSuiteRunner
    {
        Process testRunningCLI;

        protected virtual void ValidateCliCommand(string FullCLICommand)
        {
            if (FullCLICommand == null)
                throw new ArgumentNullException("FullCLICommand");

            if (FullCLICommand == string.Empty)
                throw new ArgumentException("Should not be empty string", "FullCLICommand");
        }

        protected virtual string GetProcessExecutableNameFromCommand(string FullCLICommand)
        {
            if (string.IsNullOrEmpty(FullCLICommand))
                return string.Empty;

            if (FullCLICommand.StartsWith("\""))
            {
                int indexOfSecondQuoteMark = FullCLICommand.IndexOf('"', 1);

                return FullCLICommand.Substring(0, indexOfSecondQuoteMark + 1);
            }

            int firstIndexOfSpace = FullCLICommand.IndexOf(' ');

            if (firstIndexOfSpace < 0)
                return FullCLICommand;

            return FullCLICommand.Substring(0, firstIndexOfSpace);
        }

        protected virtual string GetProcessArgumentsFromCommand(string FullCLICommand)
        {
            if (string.IsNullOrEmpty(FullCLICommand))
                return string.Empty;

            var executableName = GetProcessExecutableNameFromCommand(FullCLICommand);

            return FullCLICommand.Substring(executableName.Length).TrimStart(' ');
        }
    }
}