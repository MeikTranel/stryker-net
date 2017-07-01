using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace Stryker.NET.CLITestExecutionInterpreters.UnitTests
{
    public class NunitCLITestExecutionInterpreterTests
    {
        [Fact]
        public void ConstructorTest_NoException()
        {
            var instance = new NunitCLITestExecutionInterpreter(new MemoryStream());
        }

        [Fact]
        public void InputStreamPrintsMessageAboutSuccessfullTestExecution_UnitTestExecutionFinishedEventRaised()
        {
            //https://stackoverflow.com/questions/2784878/continuously-reading-from-a-stream
            //https://stackoverflow.com/questions/12678407/getting-command-line-output-dynamically

            var testStream = new MemoryStream();

            TextReader tr = new StreamReader(testStream);

            var subject = new NunitCLITestExecutionInterpreter(testStream);

            string expectedExecutedTestName = "OpenCover.UI.TestDiscoverer.TestResources.NUnit.RegularTestFixture.RegularTestMethod";

            string actualExecutedTestName = string.Empty;

            subject.UnitTestExecutionFinished += (object sender, TestCaseEventArgs e) =>
            {
                actualExecutedTestName = e.TestCaseName;
            };

            var sampleNunitInformation = $"##teamcity[testFinished name='{expectedExecutedTestName}' duration='5' flowId='0-1003']";

            var sampleNunitInformationBytes = Encoding.UTF8.GetBytes(sampleNunitInformation);

            var r = tr.ReadLineAsync();

            TextWriter tw = new StreamWriter(testStream);

            tw.Write(sampleNunitInformation);

            tw.Flush();

            testStream.Position = 0;

            //testStream.Write(sampleNunitInformationBytes, 0, sampleNunitInformationBytes.Length);

            actualExecutedTestName = r.Result;

            Assert.Equal(expectedExecutedTestName, actualExecutedTestName);
        }
    }
}