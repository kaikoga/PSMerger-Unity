using System.Collections.Generic;
using System.Linq;
using Silksprite.PSMerger.Compiler.Extensions;

namespace Silksprite.PSMerger.Compiler.Data
{
    public class JavaScriptCompilerEnvironment
    {
        public readonly JavaScriptInput[] ScriptLibraries;
        public readonly JavaScriptCompilerContext[] ScriptContexts;
        public readonly bool DetectCallbackSupport;

        JavaScriptCompilerEnvironment(IEnumerable<JavaScriptSource> sources, bool detectCallbackSupport, string defaultSourceCode)
        {
            var sourcesArray = sources.ToArray();
            ScriptLibraries = sourcesArray.SelectMany(source => source.ScriptLibraries)
                .ToJavaScriptInputs(defaultSourceCode)
                .ToArray();
            ScriptContexts = sourcesArray.SelectMany(source => source.ScriptContexts)
                .Select(context => new JavaScriptCompilerContext(context, defaultSourceCode))
                .ToArray();
            DetectCallbackSupport = detectCallbackSupport;
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptComponentMergerBase component, IEnumerable<JavaScriptSource> mergedSources)
        {
            var inlineJavaScript = component.gameObject.GetComponent<IInlineJavaScript>();
            return new JavaScriptCompilerEnvironment(
                component.JavaScriptSources().Concat(mergedSources),
                component.DetectCallbackSupport,
                inlineJavaScript?.SourceCode);
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptAssetMerger asset)
        {
            return new JavaScriptCompilerEnvironment(
                asset.JavaScriptSources(),
                asset.DetectCallbackSupport,
                null);
        }

        public IEnumerable<JavaScriptInput> AllInputs() => 
            ScriptLibraries.Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs));
    }
}