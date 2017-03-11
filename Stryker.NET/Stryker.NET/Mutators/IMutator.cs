using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Stryker.NET.Mutators
{
    public interface IMutator
    {
        string Name { get; }

        IEnumerable<SyntaxNode> ApplyMutations(SyntaxNode node);
    }
}