using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stryker.NET.Mutators;
using System.Collections.Generic;

namespace Stryker.NET
{
    internal class MutatorOrchestrator
    {

        public List<IMutator> Mutators { get; private set; }

        public MutatorOrchestrator()
        {
            Mutators = new List<IMutator> { new MathMutator() };
        }

        public IEnumerable<Mutant> mutate(IEnumerable<string> files)
        {
            var mutants = new List<Mutant>();

            foreach (var file in files)
            {
                var originalCode = System.IO.File.ReadAllText(file);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(originalCode);
                var root = (CompilationUnitSyntax)tree.GetRoot();
                var nodes = new List<SyntaxNode>();
                CollectNodes(root, nodes);

                foreach (var node in nodes)
                {
                    foreach (var mutator in Mutators)
                    {
                        var mutatedNode = mutator.ApplyMutations(node);
                        if (mutatedNode != null)
                        {
                            var mutatedCode = root.ReplaceNode(node, mutatedNode).ToFullString();
                            mutants.Add(new Mutant(mutator.Name, file, mutatedCode, node.ToFullString(), mutatedNode.ToFullString(), node.Span));
                        }
                    }
                }
            }

            return mutants;
        }


        private void CollectNodes(SyntaxNode node, IList<SyntaxNode> nodes)
        {
            nodes.Add(node);
            foreach (var child in node.ChildNodes())
            {
                CollectNodes(child, nodes);
            }
        }
    }
}