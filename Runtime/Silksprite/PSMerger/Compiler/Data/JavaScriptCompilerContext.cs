namespace Silksprite.PSMerger.Compiler.Data
{
    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(JavaScriptInput[] javaScriptInputs)
        {
            JavaScriptInputs = javaScriptInputs;
        }
    }
}
