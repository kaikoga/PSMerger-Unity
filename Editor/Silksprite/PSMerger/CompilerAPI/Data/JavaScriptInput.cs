namespace Silksprite.PSMerger.CompilerAPI.Data
{
    public class JavaScriptInput
    {
        public readonly string SourceCode;
        public readonly string SourceCodePath;
        public readonly string Sourcemap;

        public JavaScriptInput(string sourceCode, string sourceCodePath = null, string sourcemap = null)
        {
            SourceCode = sourceCode;
            SourceCodePath = sourceCodePath;
            Sourcemap = sourcemap;
        }

        public static JavaScriptInput Empty()
        {
            return new JavaScriptInput("");
        }

        public static JavaScriptInput Inline(string sourceCode)
        {
            return new JavaScriptInput(sourceCode);
        }
    }
}
