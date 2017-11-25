using System;
using System.Collections.Generic;
using System.Text;
using Stryker.NET.IsolatedRunner;
using Stryker.NET.Managers;
using Stryker.NET.Reporters;

namespace Stryker.NET
{
    public class InitialTestEnvironment : BaseTestEnvironment
    {
        public InitialTestEnvironment(
            IDirectoryManager directoryManager,
            string rootdir,
            string tempDir,
            string environmentName) : base(directoryManager, rootdir, tempDir, environmentName)
        {

        }

        public override bool RunTests()
        {
            return _testRunner.Test();
        }
    }
}
