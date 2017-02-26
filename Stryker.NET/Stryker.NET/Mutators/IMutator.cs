using Microsoft.CodeAnalysis;

namespace Stryker.NET.Mutators
{
    public interface IMutator
    {
        string Name { get; }
        SyntaxNode ApplyMutations(SyntaxNode node);
    }
}
