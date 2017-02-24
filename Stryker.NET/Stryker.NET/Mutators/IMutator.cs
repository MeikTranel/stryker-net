using Microsoft.CodeAnalysis;

namespace Stryker.NET.Mutators
{
    interface IMutator
    {
        string Name { get; }
        SyntaxNode ApplyMutations(SyntaxNode node);
    }
}
