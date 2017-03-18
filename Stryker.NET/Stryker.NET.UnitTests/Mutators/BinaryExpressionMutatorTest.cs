using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stryker.NET.Mutators;
using System.Linq;
using Xunit;

namespace Stryker.NET.UnitTests.Mutators
{
    public class BinaryExpressionMutatorTest
    {

        [Theory]
        [InlineData("var x = 1 + 2;", "1 - 2")]
        [InlineData("var x = 1 - 2;", "1 + 2")]
        [InlineData("var x = 1 * 2;", "1 / 2")]
        [InlineData("var x = 1 / 2;", "1 * 2")]
        [InlineData("var x = 1 % 2;", "1 * 2")]
        [InlineData("var x = 1 == 2;", "1 != 2")]
        [InlineData("var x = 1 != 2;", "1 == 2")]
        [InlineData("var x = 1 || 2;", "1 && 2")]
        [InlineData("var x = 1 && 2;", "1 || 2")]
        public void ApplyMutations_ValidBinaryExpression_ReturnsSingleMutatedBinaryExpression(string originalCode, string expectedCode)
        {
            var mutator = new BinaryExpressionMutator();
            var node = GetBinaryExpression(originalCode);

            var mutatedNodes = mutator.ApplyMutations(node);
            var mutatedCode = mutatedNodes.Single().ToFullString();
            Assert.Equal(expectedCode, mutatedCode);
        }

        [Theory]
        [InlineData("var x = 1 < 2;", new string[] { "1 <= 2", "1 >= 2" })]
        [InlineData("var x = 1 > 2;", new string[] { "1 <= 2", "1 >= 2" })]
        [InlineData("var x = 1 <= 2;", new string[] { "1 < 2", "1 > 2" })]
        [InlineData("var x = 1 >= 2;", new string[] { "1 < 2", "1 > 2" })]
        public void ApplyMutations_ValidBinaryExpression_ReturnsMultipleMutatedBinaryExpressions(string originalCode, string[] expectedMutatedCode)
        {
            var mutator = new BinaryExpressionMutator();
            var node = GetBinaryExpression(originalCode);

            var mutatedNodes = mutator.ApplyMutations(node).ToArray();

            Assert.Equal(expectedMutatedCode.Length, mutatedNodes.Length);
            for (int i = 0; i < expectedMutatedCode.Length; i++)
            {
                var mutatedCode = mutatedNodes[i].ToFullString();
                Assert.Equal(expectedMutatedCode[i], mutatedCode);
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