using System;
using System.Collections.Generic;
using System.IO;
using Stryker.NET.Managers;
using System.Text;
using Stryker.NET.Reporters;
using System.Threading.Tasks;
using Stryker.NET.Core;
using Stryker.NET.Core.Event;
using System.Linq;
using Stryker.NET.Report;

namespace Stryker.NET
{
    class Stryker : IDisposable, IMutantTestedHandler
    {
        private readonly string _tempDirNameBase = "stryker_temp";
        private readonly IDirectoryManager _directoryManager;
        private readonly IReporter _reporter;
        private readonly string _rootdir;
        private readonly string _tempDir;
        private readonly string _mutationDir;
        private TestEnvironment[] _environments = new TestEnvironment[4];
        public IEnumerable<string> _files { get; private set; }

        private event AllMutantsTestedDelegate AllMutantsTested = delegate { };
        private event ScoreCalculatedDelegate ScoreCalculated = delegate { };
        private event WrapUpDelegate WrapUp = delegate { };

        private ICollection<MutantResult> results;

        public Stryker(
            IDirectoryManager directoryManager,
            IReporter reporter,
            string rootdir)
        {
            _directoryManager = directoryManager;
            _reporter = reporter;

            _rootdir = rootdir;
            _tempDir = _tempDirNameBase;
            _mutationDir = $"{_rootdir}\\Stryker.NET";

            results = new List<MutantResult>();
            AllMutantsTested += new AllMutantsTestedDelegate(OnAllMutantsTested);
            ScoreCalculated += new ScoreCalculatedDelegate(_reporter.OnScoreCalculated);
            WrapUp += new WrapUpDelegate(_reporter.OnWrapUp);
        }

        public void RunMutationTests()
        {
            if (_reporter == null)
            {
                throw new Exception("A reporter was not specified");
            }

            Console.WriteLine("Creating sandbox for testing");
            var env = new InitialTestEnvironment(_directoryManager, _rootdir, _tempDir, "testrunner");
            env.PrepareEnvironment();
            var task = Task<bool>.Factory.StartNew(() =>
            {
                var succes = env.RunTests();
                env.Dispose();
                return succes;
            });
            Console.WriteLine("Running initial test");
            Task.WaitAll(task);

            if (!task.Result)
            {
                Console.WriteLine("Initial test FAILED!");
                return;
            }
            else
            {
                Console.WriteLine("Initial test PASSED!");
            }

            Console.WriteLine("Preparing files for mutation...");

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

            // create test environments
            for (var i = 0; i < 4; i++)
            {
                _environments[i] = new TestEnvironment(this, _directoryManager, _reporter, _rootdir, _tempDir, "environment" + i);
                _environments[i].PrepareEnvironment();
            }

            // assign mutants to test environments
            var count = 0;
            foreach (var mutant in mutants)
            {
                var entvironmentNumber = count % _environments.Length;
                _environments[entvironmentNumber].Mutants.Enqueue(mutant);
                count++;
            }

            Console.WriteLine("Start testing mutants!");
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
            
            // notify 'all mutants tested' observers
            Console.WriteLine("Done testing all mutants!");
            AllMutantsTested(results.ToList());

            // notify 'wrap up' observers
            Console.WriteLine("Wrapping up...");
            WrapUp();
        }

        public void Dispose()
        {
            _reporter?.Dispose();
            _directoryManager?.RemoveDirectory(_tempDirNameBase);
        }

        public void OnMutantTested(MutantResult result)
        {
            results.Add(result);
        }

        public void OnAllMutantsTested(IReadOnlyCollection<MutantResult> results)
        {
            var timedOut = results.Where(r => r.Status == MutantStatus.TimedOut).Count();
            var killed = results.Where(r => r.Status == MutantStatus.Killed).Count();
            var survived = results.Where(r => r.Status == MutantStatus.Survived).Count();
            var noCoverage = results.Where(r => r.Status == MutantStatus.NoCoverage).Count();

            var scoreResult = new ScoreResult(killed, timedOut, survived, noCoverage);
            
            // notify 'score calculated' observers
            ScoreCalculated(scoreResult);
        }
    }
}