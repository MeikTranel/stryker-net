using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stryker.NET.CLITestSuiteRunners.UnitTests
{
    public class NunitCLITestSuiteRunnerTests
    {
        public class ValidateCliCommand : NunitCLITestSuiteRunner
        {
            [Fact]
            public void ValidateCliCommand_NullCommand_Exception()
            {
                string command = null;

                Assert.Throws<ArgumentNullException>(() => { ValidateCliCommand(command); });
            }

            [Fact]
            public void ValidateCliCommand_EmptyCommand_Exception()
            {
                string command = string.Empty;

                Assert.Throws<ArgumentException>(() => { ValidateCliCommand(command); });
            }
        }

        public class GetProcessExecutableNameFromCommand : NunitCLITestSuiteRunner
        {
            [Theory]
            [InlineData("var x = 1 + 2;", "1 - 2")]
            public void GetProcessExecutableNameFromCommand_NullCommand_EmptyStringOutput()
            {
                string command = null;
                var expected = string.Empty;

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_EmptyCommand_EmptyStringOutput()
            {
                string command = string.Empty;
                var expected = string.Empty;

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_OneWordCommand_SameWordReturned()
            {
                string command = "SomeProgramToRun.exe";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_TwoWordCommandSeparatedWithOneSpace_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe --parametersNotProvided";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_TwoWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_ManyWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided yet   there  are     some";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpace_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided yet   there  are     some";
                var expected = @"""c:\Program Files\SomeProgramToRun.exe""";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void GetProcessExecutableNameFromCommand_ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpaceAndQuotedParemeters_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided \"yet   there  are     some\"";
                var expected = @"""c:\Program Files\SomeProgramToRun.exe""";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }
        }
    }
}