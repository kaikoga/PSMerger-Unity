using System.Linq;
using Silksprite.PSMerger.Compiler.Extensions;

namespace Silksprite.PSMerger.Compiler.Data
{
    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(JavaScriptContext context, string defaultSourceCode)
        {
            JavaScriptInputs = context.JavaScriptAssets
                .ToJavaScriptInputs(defaultSourceCode)
                .ToArray();
        }
    }
}
