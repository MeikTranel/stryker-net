using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stryker.NET.Mutators;
using System.Collections.Generic;

namespace Stryker.NET
{
    public class MutatorOrchestrator
    {
        public List<IMutator> Mutators { get; private set; }

        public MutatorOrchestrator()
        {
            Mutators = new List<IMutator> {
                new BinaryExpressionMutator()
            };
        }

        /// <summary>
        /// Generates a set of mutants using implementations of IMutators from a list of code files
        /// </summary>
        /// <param name="filePaths">The files to mutate</param>
        /// <returns>A collection of mutants</returns>
        public IEnumerable<Mutant> Mutate(IEnumerable<string> filePaths)
        {
            var mutants = new List<Mutant>();

            foreach (var filePath in filePaths)
            {
                var root = GetSyntaxTreeRootFromFile(filePath);
                var nodes = new List<SyntaxNode>();
                CollectNodes(root, nodes);

                foreach (var node in nodes)
                {
                    foreach (var mutator in Mutators)
                    {
                        var mutatedNodes = mutator.ApplyMutations(node);
                        var newMutants = GenerateMutants(mutatedNodes, mutator.Name, root, node, filePath);
                        mutants.AddRange(newMutants);
                    }
                }
            }

            return mutants;
        }

        /// <summary>
        /// Restores a piece of mutated code to it's pre-mutated state
        /// </summary>
        /// <param name="mutant"></param>
        /// <returns>The restored pre-mutated code</returns>
        public string Restore(Mutant mutant)
        {
            var code = mutant.MutatedCode;
            return code.Replace(mutant.MutatedFragment, mutant.OriginalFragment);
        }

        /// <summary>
        /// Generates a set of mutants from a newly compiled file
        /// </summary>
        /// <param name="mutatedNodes">Possible code nodes/blocks to mutate</param>
        /// <param name="mutatorName">The name of the mutator used to generate the mutants</param>
        /// <param name="root">The compiled root of the file</param>
        /// <param name="node">The node to mutate</param>
        /// <param name="filePath">The source file location</param>
        /// <returns>A collection of mutants</returns>
        private IEnumerable<Mutant> GenerateMutants(IEnumerable<SyntaxNode> mutatedNodes, string mutatorName, CompilationUnitSyntax root, SyntaxNode node, string filePath)
        {
            var mutants = new List<Mutant>();
            if (mutatedNodes != null)
            {
                foreach (var mutatedNode in mutatedNodes)
                {
                    var mutatedCode = root.ReplaceNode(node, mutatedNode).ToFullString();
                    var originalNodeLinePosition = node.GetLocation().GetLineSpan();
                    var mutant = new Mutant(
                        mutatorName, 
                        filePath, 
                        mutatedCode, 
                        node.ToFullString(), 
                        mutatedNode.ToFullString(), 
                        node.Span,
                        originalNodeLinePosition.StartLinePosition);
                    mutants.Add(mutant);
                }
            }
            return mutants;
        }

        /// <summary>
        /// Dynamically compiles a code file using Roslyn from the given filepath and returns it's compiled syntax root
        /// </summary>
        /// <param name="filePath">The path of the file to compile</param>
        /// <returns>The compiled root node of the file</returns>
        private CompilationUnitSyntax GetSyntaxTreeRootFromFile(string filePath)
        {
            var originalCode = System.IO.File.ReadAllText(filePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(originalCode);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            return root;
        }

        /// <summary>
        /// Collects and flattens tree-nodes recursively into a flat list
        /// </summary>
        /// <param name="node">The root node to start collecting from</param>
        /// <param name="nodes">The list to put the root nodes and the childnodes into</param>
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