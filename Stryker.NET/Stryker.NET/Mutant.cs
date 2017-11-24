using Microsoft.CodeAnalysis.Text;

namespace Stryker.NET
{
    public class Mutant
    {
        public string MutatedFragment { get; private set; }
        public string MutatorName { get; private set; }
        public string OriginalFragment { get; private set; }
        public string FilePath { get; private set; }
        public string MutatedCode { get; private set; }
        public TextSpan Span { get; private set; }
        public LinePosition LinePosition { get; }

        public Mutant(string mutatorName, string filePath, string mutatedCode, string originalFragment, string mutatedFragment, TextSpan span, LinePosition linePosition)
        {
            MutatorName = mutatorName;
            FilePath = filePath;
            MutatedCode = mutatedCode;
            OriginalFragment = originalFragment;
            MutatedFragment = mutatedFragment;
            Span = span;
            LinePosition = linePosition;
        }
    }
}