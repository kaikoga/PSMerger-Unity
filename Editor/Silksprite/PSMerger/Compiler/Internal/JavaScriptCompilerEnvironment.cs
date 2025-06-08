using System.Collections.Generic;
using System.Linq;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerEnvironment
    {
        public readonly JavaScriptInput[] ScriptLibraries;
        public readonly JavaScriptCompilerContext[] ScriptContexts;
        public readonly bool DetectCallbackSupport;

        public JavaScriptCompilerEnvironment(JavaScriptSource source)
        {
            ScriptLibraries = source.scriptLibraries
                .Where(asset => asset != null)
                .Select(asset => new JavaScriptInput(asset))
                .ToArray();
            ScriptContexts = source.scriptContexts
                .Select(context => new JavaScriptCompilerContext(context))
                .ToArray();
            DetectCallbackSupport = source.detectCallbackSupport;
        }
        
        public IEnumerable<JavaScriptInput> AllInputs() => 
            ScriptLibraries.Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs));
    }

    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(JavaScriptContext context)
        {
            JavaScriptInputs = context.JavaScriptAssets
                .Where(asset => asset != null)
                .Select(asset => new JavaScriptInput(asset))
                .ToArray();
        }
    }

}
