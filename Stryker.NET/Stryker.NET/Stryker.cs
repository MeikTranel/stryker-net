using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;
using Stryker.NET.Reporters;
using Stryker.NET.Core;

namespace Stryker.NET
{
    class Stryker : IDisposable
    {
        public delegate void MutantTestedDelegate(MutantResult result);
        public delegate void AllMutantsTestedDelegate(IReadOnlyCollection<MutantResult> result);
        public delegate void ScoreCalculatedDelegate();
        public delegate void WrapUpDelegate();

        private readonly string _tempDirName = "stryker_temp";

        private readonly IDirectoryManager _directoryManager;
        private readonly IReporter _reporter;
        private readonly ITestRunner _testRunner;
        private readonly string _rootdir;
        private readonly string _tempDir;
        private readonly string _mutationDir;
        public IEnumerable<string> _files { get; private set; }
        
        private event MutantTestedDelegate MutantTested;
        private event AllMutantsTestedDelegate AllMutantsTested;
        private event ScoreCalculatedDelegate ScoreCalculated;
        private event WrapUpDelegate WrapUp;

        public Stryker(ITestRunner testRunner, 
            IDirectoryManager directoryManager,
            IReporter reporter,
            string rootdir)
        {
            _testRunner = testRunner;
            _directoryManager = directoryManager;
            _reporter = reporter;

            _rootdir = rootdir;
            _tempDir = $"{rootdir}\\{_tempDirName}";
            _mutationDir = $"{_tempDir}\\Stryker.NET";
            
            MutantTested += new MutantTestedDelegate(_reporter.OnMutantTested);
            AllMutantsTested += new AllMutantsTestedDelegate(_reporter.OnAllMutantsTested);
            WrapUp += new WrapUpDelegate(_reporter.OnWrapUp);
        }

        public void PrepareEnvironment()
        {
            _directoryManager.CopyRoot(_rootdir, _tempDir);
        }

        public void RunMutationTest()
        {
            if (_testRunner == null)
            {
                throw new Exception("A test runner was not specified");
            }
            if (_reporter == null)
            {
                throw new Exception("A reporter was not specified");
            }

            _files = _directoryManager.GetFiles(_mutationDir);

            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(_files);

            var results = new List<MutantResult>();
            foreach (var mutant in mutants)
            {
                var path = mutant.FilePath;

                // (over)write temp code file
                File.WriteAllText(path, mutant.MutatedCode, Encoding.Unicode);

                // run unit tests with mutant
                _testRunner.Test();

                // create and store mutant result
                var status = MutantStatus.Killed;
                var mutantResult = new MutantResult(
                    mutant.FilePath,
                    mutant.MutatorName,
                    status,
                    mutant.MutatedCode,
                    mutant.LinePosition.Line.ToString(), 
                    mutant.LinePosition.Line.ToString(), //TODO: get correct mutated line position
                    null,
                    new Location(
                        new Position(mutant.LinePosition.Line, mutant.LinePosition.Character),
                        new Position(mutant.LinePosition.Line, mutant.LinePosition.Character) //TODO: get correct mutated line position
                        ),
                    new Range<int>(0, 1)); //TODO: get correct mutated range

                results.Add(mutantResult);

                // notify 'mutant tested' observers
                MutantTested(mutantResult);

                // restore file to original state
                string restoredCode = mutatorOrchestrator.Restore(mutant);
                File.WriteAllText(path, restoredCode);
            }

            // notify 'score calculated' observers
            ScoreCalculated();

            // notify 'all mutants tested' observers
            AllMutantsTested(results);

            // notify 'wrap up' observers
            WrapUp();
        }

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
            _reporter?.Dispose();
        }
    }
}