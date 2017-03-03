using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Stryker.NET.Mutators
{
    public class BinaryExpressionMutator : IMutator
    {
        public Dictionary<SyntaxKind, IEnumerable<SyntaxKind>> Tokens { get; private set; }

        public string Name { get { return "BinaryExpression"; } }

        public BinaryExpressionMutator()
        {
            Tokens = new Dictionary<SyntaxKind, IEnumerable<SyntaxKind>>
            {
                {SyntaxKind.MinusToken, new List<SyntaxKind> { SyntaxKind.PlusToken } },
                {SyntaxKind.PlusToken, new List<SyntaxKind> {SyntaxKind.MinusToken } },
                {SyntaxKind.AsteriskToken, new List<SyntaxKind> {SyntaxKind.SlashToken } },
                {SyntaxKind.SlashToken, new List<SyntaxKind> {SyntaxKind.AsteriskToken } },
                {SyntaxKind.PercentToken, new List<SyntaxKind> {SyntaxKind.AsteriskToken } },
                {SyntaxKind.LessThanToken, new List<SyntaxKind> {SyntaxKind.LessThanEqualsToken, SyntaxKind.GreaterThanEqualsToken} },
                {SyntaxKind.GreaterThanToken, new List<SyntaxKind> {SyntaxKind.LessThanEqualsToken, SyntaxKind.GreaterThanEqualsToken} },
                {SyntaxKind.LessThanEqualsToken, new List<SyntaxKind> { SyntaxKind.LessThanToken, SyntaxKind.GreaterThanToken } },
                {SyntaxKind.GreaterThanEqualsToken, new List<SyntaxKind> { SyntaxKind.LessThanToken, SyntaxKind.GreaterThanToken } },
                {SyntaxKind.EqualsEqualsToken, new List<SyntaxKind> {SyntaxKind.ExclamationEqualsToken } },
                {SyntaxKind.ExclamationEqualsToken, new List<SyntaxKind> {SyntaxKind.EqualsEqualsToken } },
                {SyntaxKind.BarBarToken, new List<SyntaxKind> {SyntaxKind.AmpersandAmpersandToken } },
                {SyntaxKind.AmpersandAmpersandToken, new List<SyntaxKind> {SyntaxKind.BarBarToken } },
            };
        }

        public IEnumerable<SyntaxNode> ApplyMutations(SyntaxNode node)
        {
            IList<SyntaxNode> mutatedNodes = null;

            var binaryNode = node as BinaryExpressionSyntax;
            if (binaryNode != null && Tokens.ContainsKey(binaryNode.OperatorToken.Kind()))
            {
                mutatedNodes = new List<SyntaxNode>();
                foreach (var mutatedOperatorToken in Tokens[binaryNode.OperatorToken.Kind()])
                {
                    var mutatedOperator = SyntaxFactory.Token(mutatedOperatorToken).WithTrailingTrivia(binaryNode.OperatorToken.TrailingTrivia);
                    mutatedNodes.Add(binaryNode.WithOperatorToken(mutatedOperator));
                }
            }
            return mutatedNodes;
        }
    }
}
