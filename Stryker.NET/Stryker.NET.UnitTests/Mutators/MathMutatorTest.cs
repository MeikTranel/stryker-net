using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Stryker.NET.Mutators;
using System.Linq;

namespace Stryker.NET.UnitTests.Mutators
{
    [TestFixture]
    public class MathMutatorTest
    {
        private MathMutator mutator;

        [SetUp]
        public void Setup()
        {
            mutator = new MathMutator();
        }

        [TestCase("var x = 1 + 2;", "1 - 2")]
        [TestCase("var x = 1 - 2;", "1 + 2")]
        [TestCase("var x = 1 * 2;", "1 / 2")]
        [TestCase("var x = 1 / 2;", "1 * 2")]
        [TestCase("var x = 1 % 2;", "1 * 2")]
        public void ApplyMutations_ValidBinaryExpression_ReturnsMutatedBinaryExpression(string originalCode, string expectedMutatedCode)
        {
            var node = GetBinaryExpression(originalCode);

            var mutatedNode = mutator.ApplyMutations(node);
            var mutatedCode = mutatedNode.ToFullString();

            Assert.AreEqual(expectedMutatedCode, mutatedCode);
        }

        private SyntaxNode GetBinaryExpression(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var fieldDeclaration = root.ChildNodes().First();
            var variableDeclaration = fieldDeclaration.ChildNodes().First();
            var variableDeclarator = variableDeclaration.ChildNodes().First(c => c.IsKind(SyntaxKind.VariableDeclarator));
            var equalsValueClause = variableDeclarator.ChildNodes().First();
            var binaryExpression = equalsValueClause.ChildNodes().First();

            return binaryExpression;
        }
    }
}
