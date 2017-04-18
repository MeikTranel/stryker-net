using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SampleLogicForTests;

namespace SampleLogicNunitTests
{
    [TestFixture]
    public class SampleLogicClassTests
    {
        [Test]
        public void WriteTwice_NullInput_EmptyOutput()
        {
            var subject = new SampleLogicClass();

            string data = null;

            var expected = string.Empty;

            var actual = subject.WriteTwice(data);

            Assert.AreEqual(expected, actual);
        }
    }
}