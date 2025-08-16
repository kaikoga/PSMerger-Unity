using System.Collections.Generic;
using System.Linq;

namespace Silksprite.PSMerger.CompilerAPI.Data
{
    public class JavaScriptCompilerEnvironment
    {
        public readonly JavaScriptInput[] ScriptLibraries;
        public readonly JavaScriptCompilerContext[] ScriptContexts;
        public readonly bool DetectCallbackSupport;

        public JavaScriptCompilerEnvironment(IEnumerable<JavaScriptInput> libraries, IEnumerable<JavaScriptCompilerContext> contexts, bool detectCallbackSupport)
        {
            ScriptLibraries = libraries.ToArray();
            ScriptContexts = contexts.ToArray();
            DetectCallbackSupport = detectCallbackSupport;
        }

        public IEnumerable<JavaScriptInput> AllInputs() => 
            ScriptLibraries.Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs));
    }
}