using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Compiler.Data;

namespace Silksprite.PSMerger.Compiler.Extension
{
    public static class JavaScriptCompilerEnvironmentFactory
    {
        public static JavaScriptCompilerEnvironment Create(IEnumerable<JavaScriptSource> sources, bool detectCallbackSupport, string defaultSourceCode)
        {
            var sourcesArray = sources.ToArray();
            var libraries = sourcesArray.SelectMany(source => source.ScriptLibraries)
                .ToJavaScriptInputs(defaultSourceCode)
                .ToArray();
            var contexts = sourcesArray.SelectMany(source => source.ScriptContexts)
                .Select(context => CreateContext(context, defaultSourceCode))
                .ToArray();
            return new JavaScriptCompilerEnvironment(libraries, contexts, detectCallbackSupport);
        }

        static JavaScriptCompilerContext CreateContext(JavaScriptContext context, string defaultSourceCode)
        {
            var javaScriptInputs = context.JavaScriptAssets
                .ToJavaScriptInputs(defaultSourceCode)
                .ToArray();
            return new JavaScriptCompilerContext(javaScriptInputs);
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