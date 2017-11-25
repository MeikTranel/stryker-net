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
        private TestEnvironment[] _environments = new TestEnvironment[4];
        public IEnumerable<string> _files { get; private set; }

        public Stryker(IDirectoryManager directoryManager, 
            IReporter reporter,
            string rootdir)
        {
            _directoryManager = directoryManager;
            _reporter = reporter;
            _rootdir = rootdir;
            _tempDir = _tempDirNameBase;
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

            // create test environments
            for (int i = 0; i < 4; i++)
            {
                _environments[i] = new TestEnvironment(_directoryManager, _reporter, _rootdir, _tempDir, "environment" + i);
                _environments[i].PrepareEnvironment();
            }

            // assign mutants to test environments
            int count = 0;
            foreach (var mutant in mutants)
            {
                var entvironmentNumber = count % 4;
                _environments[entvironmentNumber].Mutants.Enqueue(mutant);
                count++;
            }

            // start testing task in each environment
            var testRuns = new List<Task>();
            foreach (var environment in _environments)
            {
                testRuns.Add(Task.Factory.StartNew(() =>
                {
                    environment.RunTests();
                    environment.Dispose();
                }));
            }
            Task.WaitAll(testRuns.ToArray());
            Console.WriteLine("testRuns done");
        }

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
            _reporter?.Dispose();
            _directoryManager?.RemoveDirectory(_tempDirNameBase);
        }
    }
}