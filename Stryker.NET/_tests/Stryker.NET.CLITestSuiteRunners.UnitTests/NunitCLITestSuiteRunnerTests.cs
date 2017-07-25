using System;
using Xunit;

namespace Stryker.NET.CLITestSuiteRunners.UnitTests
{
    public class NunitCLITestSuiteRunnerTests
    {
        public class Constructor
        {
            [Fact]
            public void ProcessWithoutArguments_NoException()
            {
                new NunitCLITestSuiteRunner("some.exe");
            }

            [Fact]
            public void ProcessWithArguments_NoExceptionAndProcessExeAndArgsSet()
            {
                var subject = new NunitCLITestSuiteRunner("some.exe some arguments");
                Assert.Equal("some.exe", subject.TestSuiteRunnerExecutable);
                Assert.Equal("some arguments", subject.TestSuiteRunnerArguments);
            }
        }

        public class StartTestSuiteExecution
        {
            [Fact]
            public void ProcessReturningNothing_NoEventsRaisedAndExceptionInTheEnd()
            {
                var subject = new NunitCLITestSuiteRunner("powershell.exe -command exit");

                subject.UnitTestExecutionStarted += (object sender, TestCaseEventArgs e) => { throw new Exception("This should not be invoked"); };
                subject.UnitTestExecutionFinished += (object sender, TestCaseEventArgs e) => { throw new Exception("This should not be invoked"); };

                Assert.Throws<Exception>(() => { subject.StartTestSuiteExecution(); });
            }

            [Fact]
            public void ProcessReturningSomeOutputIrrelevantToUnitTest_NoEventsRaisedAndExceptionInTheEnd()
            {
                var subject = new NunitCLITestSuiteRunner("powershell.exe -command Write-Output some text irrelevant to unit tests ; exit");

                subject.UnitTestExecutionStarted += (object sender, TestCaseEventArgs e) => { throw new Exception("This should not be invoked"); };
                subject.UnitTestExecutionFinished += (object sender, TestCaseEventArgs e) => { throw new Exception("This should not be invoked"); };

                Assert.Throws<Exception>(() => { subject.StartTestSuiteExecution(); });
            }

            [Fact]
            public void ProcessReturningNunitTestStartedMessage_UnitTestExecutionStartedEventRaised()
            {
                var unitTestStartedMessage = "##teamcity[testStarted name='PassingCars.Tests.SolutionWithTestsTests2.solution_MaximumInputSizeAllZeros_0' captureStandardOutput='false' flowId='1']";
                var subject = new NunitCLITestSuiteRunner($"powershell.exe -command Write-Output {unitTestStartedMessage} ; exit");

                bool UnitTestExecutionStartedEventRaised = false;

                subject.UnitTestExecutionStarted += (object sender, TestCaseEventArgs e) => { UnitTestExecutionStartedEventRaised = true; };
                subject.UnitTestExecutionFinished += (object sender, TestCaseEventArgs e) => { throw new Exception("This should not be invoked"); };

                Assert.True(UnitTestExecutionStartedEventRaised, "UnitTestExecutionStarted should be invoked but it wasnt");
            }
        }

        public class ValidateCliCommand : NunitCLITestSuiteRunner
        {
            [Fact]
            public void NullCommand_Exception()
            {
                string command = null;

                Assert.Throws<ArgumentNullException>(() => { ValidateCliCommand(command); });
            }

            [Fact]
            public void EmptyCommand_Exception()
            {
                string command = string.Empty;

                Assert.Throws<ArgumentException>(() => { ValidateCliCommand(command); });
            }
        }

        public class GetProcessExecutableNameFromCommand : NunitCLITestSuiteRunner
        {
            [Fact]
            public void NullCommand_EmptyStringOutput()
            {
                string command = null;
                var expected = string.Empty;

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void EmptyCommand_EmptyStringOutput()
            {
                string command = string.Empty;
                var expected = string.Empty;

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void OneWordCommand_SameWordReturned()
            {
                string command = "SomeProgramToRun.exe";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TwoWordCommandSeparatedWithOneSpace_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe --parametersNotProvided";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TwoWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided yet   there  are     some";
                var expected = "SomeProgramToRun.exe";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpace_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided yet   there  are     some";
                var expected = @"""c:\Program Files\SomeProgramToRun.exe""";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpaceAndQuotedParemeters_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided \"yet   there  are     some\"";
                var expected = @"""c:\Program Files\SomeProgramToRun.exe""";

                var actual = GetProcessExecutableNameFromCommand(command);

                Assert.Equal(expected, actual);
            }
        }

        public class GetProcessArgumentsFromCommand : NunitCLITestSuiteRunner
        {
            [Fact]
            public void NullCommand_EmptyStringOutput()
            {
                string command = null;
                var expected = string.Empty;

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void EmptyCommand_EmptyStringOutput()
            {
                string command = string.Empty;
                var expected = string.Empty;

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void OneWordCommand_SameWordReturned()
            {
                string command = "SomeProgramToRun.exe";
                var expected = "";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TwoWordCommandSeparatedWithOneSpace_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe --parametersNotProvided";
                var expected = "--parametersNotProvided";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TwoWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided";
                var expected = "--parametersNotProvided";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpaces_FirstWordWithoutSpacesReturned()
            {
                string command = "SomeProgramToRun.exe           --parametersNotProvided yet   there  are     some";
                var expected = "--parametersNotProvided yet   there  are     some";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpace_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided yet   there  are     some";
                var expected = @"--parametersNotProvided yet   there  are     some";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ManyWordCommandSeparatedWithManySpacesWithQuotedFullExecutablePathContainingSpaceAndQuotedParemeters_FirstWordInQuotesWithoutSpacesOnTheEndReturned()
            {
                string command = "\"c:\\Program Files\\SomeProgramToRun.exe\"           --parametersNotProvided \"yet   there  are     some\"";
                var expected = "--parametersNotProvided \"yet   there  are     some\"";

                var actual = GetProcessArgumentsFromCommand(command);

                Assert.Equal(expected, actual);
            }
        }
    }
}