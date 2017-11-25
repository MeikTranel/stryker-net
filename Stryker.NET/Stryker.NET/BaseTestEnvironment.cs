using System;
using System.Collections.Generic;
using System.Text;
using Stryker.NET.IsolatedRunner;
using Stryker.NET.Managers;

namespace Stryker.NET
{
    public abstract class BaseTestEnvironment : IDisposable
    {
        protected string _tempDir;
        protected readonly string _rootdir;
        protected readonly string _environmentName;
        protected readonly ITestRunner _testRunner;
        protected readonly IDirectoryManager _directoryManager;

        public BaseTestEnvironment(
            IDirectoryManager directoryManager,
            string rootdir,
            string tempDir,
            string environmentName)
        {
            _directoryManager = directoryManager;
            _rootdir = rootdir;
            _tempDir = rootdir + "\\" + tempDir + "\\" + environmentName;
            _environmentName = environmentName;
            _testRunner = new TestRunner(_tempDir);
        }

        public void PrepareEnvironment()
        {
            // copy all files to dedicated temp dir
            _directoryManager.CopyRoot(_rootdir, _tempDir);
        }

        public abstract bool RunTests();

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
        }
    }
}
