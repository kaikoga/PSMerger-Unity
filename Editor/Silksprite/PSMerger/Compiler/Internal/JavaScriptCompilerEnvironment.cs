using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClusterVR.CreatorKit.Item;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerEnvironment
    {
        public readonly JavaScriptInput[] ScriptLibraries;
        public readonly JavaScriptCompilerContext[] ScriptContexts;
        public readonly bool DetectCallbackSupport;

        public readonly JavaScriptCompilerOutput Output;

        JavaScriptCompilerEnvironment(IEnumerable<JavaScriptSource> sources, bool detectCallbackSupport, string defaultSourceCode, JavaScriptAsset assetOutput)
        {
            var sourcesArray = sources.ToArray();
            ScriptLibraries = sourcesArray.SelectMany(source => source.ScriptLibraries)
                .ToJavaScriptInputs(defaultSourceCode)
                .ToArray();
            ScriptContexts = sourcesArray.SelectMany(source => source.ScriptContexts)
                .Select(context => new JavaScriptCompilerContext(context, defaultSourceCode))
                .ToArray();
            DetectCallbackSupport = detectCallbackSupport;
            Output = JavaScriptCompilerOutput.CreateFromAssetOutput(assetOutput);
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptComponentMergerBase component, IEnumerable<JavaScriptSource> mergedSources)
        {
            var inlineJavaScript = component.gameObject.GetComponent<IInlineJavaScript>();
            return new JavaScriptCompilerEnvironment(
                component.JavaScriptSources().Concat(mergedSources),
                component.DetectCallbackSupport,
                inlineJavaScript?.SourceCode,
                component.MergedScript);
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptAssetMerger asset)
        {
            return new JavaScriptCompilerEnvironment(
                asset.JavaScriptSources(),
                asset.DetectCallbackSupport,
                null,
                asset.MergedScript);
        }

        public IEnumerable<JavaScriptInput> AllInputs() => 
            ScriptLibraries.Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs));
    }

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

    static class JavaScriptAssetExtension
    {
        public static IEnumerable<JavaScriptInput> ToJavaScriptInputs(this IEnumerable<JavaScriptAsset> assets, string defaultSourceCode)
        {
            return assets.Select(asset => asset
                ? JavaScriptInput.FromAsset(asset)
                : !string.IsNullOrEmpty(defaultSourceCode)
                    ? JavaScriptInput.Inline(defaultSourceCode)
                    : null)
                .Where(input => input != null);
        }
    }
}
