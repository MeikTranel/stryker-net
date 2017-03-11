using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Stryker.NET.Mutators;
using System.Linq;

namespace Stryker.NET.UnitTests.Mutators
{
    [TestFixture]
    public class BinaryExpressionMutatorTest
    {
        private BinaryExpressionMutator mutator;

        [SetUp]
        public void Setup()
        {
            mutator = new BinaryExpressionMutator();
        }

        [TestCase("var x = 1 + 2;", "1 - 2")]
        [TestCase("var x = 1 - 2;", "1 + 2")]
        [TestCase("var x = 1 * 2;", "1 / 2")]
        [TestCase("var x = 1 / 2;", "1 * 2")]
        [TestCase("var x = 1 % 2;", "1 * 2")]
        [TestCase("var x = 1 == 2;", "1 != 2")]
        [TestCase("var x = 1 != 2;", "1 == 2")]
        [TestCase("var x = 1 || 2;", "1 && 2")]
        [TestCase("var x = 1 && 2;", "1 || 2")]
        public void ApplyMutations_ValidBinaryExpression_ReturnsSingleMutatedBinaryExpression(string originalCode, string expectedMutatedCode)
        {
            var node = GetBinaryExpression(originalCode);

            var mutatedNodes = mutator.ApplyMutations(node);
            var mutatedCode = mutatedNodes.Single().ToFullString();

            Assert.AreEqual(expectedMutatedCode, mutatedCode);
        }

        [TestCase("var x = 1 < 2;", new string[] { "1 <= 2", "1 >= 2" })]
        [TestCase("var x = 1 > 2;", new string[] { "1 <= 2", "1 >= 2" })]
        [TestCase("var x = 1 <= 2;", new string[] { "1 < 2", "1 > 2" })]
        [TestCase("var x = 1 >= 2;", new string[] { "1 < 2", "1 > 2" })]
        public void ApplyMutations_ValidBinaryExpression_ReturnsMultipleMutatedBinaryExpressions(string originalCode, string[] expectedMutatedCode)
        {
            var node = GetBinaryExpression(originalCode);

            var mutatedNodes = mutator.ApplyMutations(node).ToArray();

            Assert.AreEqual(expectedMutatedCode.Length, mutatedNodes.Length);
            for (int i = 0; i < expectedMutatedCode.Length; i++)
            {
                var mutatedCode = mutatedNodes[i].ToFullString();
                Assert.AreEqual(expectedMutatedCode[i], mutatedCode);
            }
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