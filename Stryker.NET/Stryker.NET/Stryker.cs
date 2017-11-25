using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;
using Stryker.NET.Reporters;
using Stryker.NET.Core;
using Stryker.NET.Core.Event;
using System.Linq;
using Stryker.NET.Report;

namespace Stryker.NET
{
    class Stryker : IDisposable
    {
        private readonly string _tempDirName = "stryker_temp";

        private readonly IDirectoryManager _directoryManager;
        private readonly IReporter _reporter;
        private readonly ITestRunner _testRunner;
        private readonly string _rootdir;
        private readonly string _tempDir;
        private readonly string _mutationDir;
        public IEnumerable<string> _files { get; private set; }
        
        private event MutantTestedDelegate MutantTested = delegate { };
        private event AllMutantsTestedDelegate AllMutantsTested = delegate { };
        private event ScoreCalculatedDelegate ScoreCalculated = delegate { };
        private event WrapUpDelegate WrapUp = delegate { };

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
            ScoreCalculated += new ScoreCalculatedDelegate(_reporter.OnScoreCalculated);
            WrapUp += new WrapUpDelegate(_reporter.OnWrapUp);
        }

        public void PrepareEnvironment()
        {
            Console.WriteLine("Preparing files for mutation...");
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

            // get files
            Console.WriteLine($"Getting files to mutate from {_mutationDir}...");
            _files = _directoryManager.GetFiles(_mutationDir);
            Console.WriteLine($"Found { _files.Count() } files.");

            // selection process 
            var filesToMutate = _files;

            // mutate
            Console.WriteLine($"Mutating {filesToMutate.Count()} files...");
            var mutatorOrchestrator = new MutatorOrchestrator();
            var mutants = mutatorOrchestrator.Mutate(_files);
            Console.WriteLine($"Created { mutants.Count() } mutants.");
            Console.WriteLine();

            Console.WriteLine("Start testing mutants!");
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
                    mutant.OriginalFragment, 
                    mutant.MutatedFragment,
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

            var timedOut = results.Where(r => r.Status == MutantStatus.TimedOut).Count();
            var killed = results.Where(r => r.Status == MutantStatus.Killed).Count();
            var survived = results.Where(r => r.Status == MutantStatus.Survived).Count();
            var noCoverage = results.Where(r => r.Status == MutantStatus.NoCoverage).Count();

            var scoreResult = new ScoreResult(killed, timedOut, survived, noCoverage);

            // notify 'all mutants tested' observers
            Console.WriteLine("Done testing all mutants!");
            AllMutantsTested(results);

            // notify 'score calculated' observers
            ScoreCalculated(scoreResult);

            // notify 'wrap up' observers
            Console.WriteLine("Wrapping up...");
            WrapUp();
        }

        public void Dispose()
        {
            _directoryManager.RemoveDirectory(_tempDir);
            _reporter?.Dispose();
        }
    }
}