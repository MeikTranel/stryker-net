using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;
using Stryker.NET.Reporters;
using System.Threading.Tasks;

namespace Stryker.NET
{
    class Stryker : IDisposable
    {
        private readonly string _tempDirNameBase = "stryker_temp";

        private readonly IDirectoryManager _directoryManager;
        private readonly IReporter _reporter;
        private readonly string _rootdir;
        private readonly string _tempDir;
        private readonly string _mutationDir;
        public IEnumerable<string> _files { get; private set; }

        public Stryker(IDirectoryManager directoryManager, 
            IReporter reporter,
            string rootdir)
        {
            _directoryManager = directoryManager;
            _reporter = reporter;
            _rootdir = rootdir;
            _tempDir = $"{rootdir}\\{_tempDirNameBase}";
            _mutationDir = $"{_rootdir}\\Stryker.NET";
        }
        
        public void RunMutationTests()
        {
            if (_reporter == null)
            {
                throw new Exception("A reporter was not specified");
            }

            _files = _directoryManager.GetFiles(_mutationDir);

            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(_files);

            foreach (var mutant in mutants)
            {
                Task.Factory.StartNew(() =>
                {
                    using (var testEnvironment = new TestEnvironment(mutant, _directoryManager, _reporter, _rootdir, _tempDir))
                    {
                        testEnvironment.PrepareEnvironment();
                        testEnvironment.RunTest();
                    }
                    //var path = mutant.FilePath;
                    // restore mutant to original state
                    //string restoredCode = mutatorOrchestrator.Restore(mutant);
                    //File.WriteAllText(path, restoredCode);
                });
            }
        }

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
            _reporter?.Dispose();
        }
    }
}