using System.Collections.Generic;
using System.Linq;

namespace Silksprite.PSMerger.CompilerAPI.Data
{
    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(IEnumerable<JavaScriptInput> javaScriptInputs)
        {
            JavaScriptInputs = javaScriptInputs.ToArray();
        }
    }
}
