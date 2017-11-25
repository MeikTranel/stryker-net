using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Stryker.NET.IsolatedRunner;
using Stryker.NET.Managers;
using Stryker.NET.Reporters;

namespace Stryker.NET
{
    public class TestEnvironment : IDisposable
    {
        private string _tempDir;
        private readonly string _rootdir;
        private readonly string _environmentName;
        private readonly IReporter _reporter;
        private readonly ITestRunner _testRunner;
        private readonly IDirectoryManager _directoryManager;

        public Queue<Mutant> Mutants = new Queue<Mutant>();

        public TestEnvironment(IDirectoryManager directoryManager,
            IReporter reporter,
            string rootdir,
            string tempDir,
            string environmentName)
        {
            _directoryManager = directoryManager;
            _rootdir = rootdir;
            _tempDir = rootdir + "\\" + tempDir + "\\" + environmentName;
            _environmentName = environmentName;
            _reporter = reporter;
            _testRunner = new TestRunner(_tempDir);
        }

        public void PrepareEnvironment()
        {
            // copy all files to dedicated temp dir
            _directoryManager.CopyRoot(_rootdir, _tempDir);
        }

        public void RunTests()
        {
            if (_testRunner == null)
            {
                throw new Exception("A test runner was not specified");
            }
            Mutant currentMutant = null;
            while (Mutants.Count > 0)
            {
                Console.WriteLine($"{_environmentName}: New testrun started");
                currentMutant = Mutants.Dequeue();
                // overwrite temp code file with mutated code
                File.WriteAllText(_tempDir + "\\" + _environmentName + "\\" + currentMutant.FilePath, currentMutant.MutatedCode, Encoding.Unicode);

                // run unit tests
                _testRunner.Test();
                _reporter.Report(currentMutant);
            }
        }

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
        }
    }
}
