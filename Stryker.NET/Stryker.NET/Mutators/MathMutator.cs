using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Stryker.NET.Mutators
{
    public class MathMutator : IMutator
    {
        public Dictionary<SyntaxKind, SyntaxKind> Tokens { get; private set; }

        public string Name { get { return "Math"; } }

        public MathMutator()
        {
            Tokens = new Dictionary<SyntaxKind, SyntaxKind>
            {
                {SyntaxKind.MinusToken, SyntaxKind.PlusToken },
                {SyntaxKind.PlusToken, SyntaxKind.MinusToken },
                {SyntaxKind.AsteriskToken, SyntaxKind.SlashToken },
                {SyntaxKind.SlashToken, SyntaxKind.AsteriskToken },
                {SyntaxKind.PercentToken, SyntaxKind.AsteriskToken }
            };
        }


        public SyntaxNode ApplyMutations(SyntaxNode node)
        {
            SyntaxNode mutatedNode = null;

            var binaryNode = node as BinaryExpressionSyntax;
            if (binaryNode != null && Tokens.ContainsKey(binaryNode.OperatorToken.Kind()))
            {
                var mutatedOperatorToken = Tokens[binaryNode.OperatorToken.Kind()];
                var mutatedOperator = SyntaxFactory.Token(mutatedOperatorToken).WithTrailingTrivia(binaryNode.OperatorToken.TrailingTrivia);
                mutatedNode = binaryNode.WithOperatorToken(mutatedOperator);
            }
            return mutatedNode;
        }
    }
}
