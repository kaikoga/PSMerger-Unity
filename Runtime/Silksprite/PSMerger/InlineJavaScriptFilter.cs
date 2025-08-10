using System.Linq;
using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.Compiler.Filter;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class InlineJavaScriptFilter : IPSMergerFilter
    {
        [InitializeOnLoadMethod]
        static void Register()
        {
            PSMergerFilter.Register(new InlineJavaScriptFilter());
        }

        public int Priority => -1000;
        public JavaScriptCompilerEnvironment Filter(JavaScriptCompilerEnvironment environment, Object unityContext)
        {
            if (unityContext is not Component component || !component.TryGetComponent<InlineJavaScript>(out var inlineJavaScript))
            {
                return environment;
            }

            var inlineInput = JavaScriptInput.Inline(inlineJavaScript.SourceCode);
            return new JavaScriptCompilerEnvironment(
                environment.ScriptLibraries.Select(input => ApplyInlineJavaScript(input, inlineInput)),
                environment.ScriptContexts.Select(context => new JavaScriptCompilerContext(context.JavaScriptInputs.Select(input => ApplyInlineJavaScript(input, inlineInput)))),
                environment.DetectCallbackSupport);
        }
        static JavaScriptInput ApplyInlineJavaScript(JavaScriptInput input, JavaScriptInput inlineInput)
        {
            return input.SourceCodePath != null ? input : inlineInput;
        }

        public string PostProcess(string sourceCode, Object unityContext)
        {
            return sourceCode;
        }
    }
}
