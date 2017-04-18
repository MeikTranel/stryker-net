using SampleLogicForTests;
using Xunit;

namespace SampleLogicxUnitTests
{
    public class SampleLogicClassTests
    {
        [Fact]
        public void WriteTwice_NullInput_EmptyOutput()
        {
            var subject = new SampleLogicClass();

            string data = null;

            var expected = string.Empty;

            var actual = subject.WriteTwice(data);

            Assert.Equal(expected, actual);
        }
    }
}