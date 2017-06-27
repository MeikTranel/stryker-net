using System;
using System.Collections.Generic;
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
            var testStream = new MemoryStream();

            var subject = new NunitCLITestExecutionInterpreter(testStream);

            subject.UnitTestExecutionFinished += (sender, e) =>
             {
                 throw new NotImplementedException("Must assert some 'e' variable features");
                 Assert.False(true); //??
             };

            var sampleNunitInformation = "##teamcity[testFinished name='OpenCover.UI.TestDiscoverer.TestResources.NUnit.RegularTestFixture.RegularTestMethod' duration='5' flowId='0-1003']";

            var sampleNunitInformationBytes = Encoding.UTF8.GetBytes(sampleNunitInformation);

            testStream.Write(sampleNunitInformationBytes, 0, sampleNunitInformationBytes.Length);

            throw new NotImplementedException("Must trigger event handler by stream changes");
        }
    }
}