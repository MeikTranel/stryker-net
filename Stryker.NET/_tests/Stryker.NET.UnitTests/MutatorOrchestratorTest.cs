using Xunit;
using System.IO;
using System.Linq;

namespace Stryker.NET.UnitTests
{
    public class MutatorOrchestratorTest
    {
        [Fact]
        public void Mutate_FileWithOnePossibleMutation_ReturnsOneMutant()
        {
            var orchestrator = new MutatorOrchestrator();
            var file = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\FileToMutate.cs");

            var mutants = orchestrator.Mutate(new[] { file });

            Assert.Equal(1, mutants.Count());
        }

        [Fact]
        public void Mutate_FileWithNoPossibleMutation_ReturnsEmpyList()
        {
            var orchestrator = new MutatorOrchestrator();
            var file = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\UnmutableFile.cs");

            var mutants = orchestrator.Mutate(new[] { file });

            Assert.Equal(0, mutants.Count());
        }
    }
}
