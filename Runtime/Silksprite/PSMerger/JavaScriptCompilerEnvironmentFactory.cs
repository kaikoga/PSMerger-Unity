using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Compiler.Data;

namespace Silksprite.PSMerger.Compiler.Extension
{
    public static class JavaScriptCompilerEnvironmentFactory
    {
        public static JavaScriptCompilerEnvironment Create(IEnumerable<JavaScriptSource> sources, bool detectCallbackSupport)
        {
            var sourcesArray = sources.ToArray();
            var libraries = sourcesArray.SelectMany(source => source.ScriptLibraries)
                .ToJavaScriptInputs()
                .ToArray();
            var contexts = sourcesArray.SelectMany(source => source.ScriptContexts)
                .Select(ToCompilerContext)
                .ToArray();
            return new JavaScriptCompilerEnvironment(libraries, contexts, detectCallbackSupport);
        }

        static JavaScriptCompilerContext ToCompilerContext(JavaScriptContext context)
        {
            return new JavaScriptCompilerContext(context.JavaScriptAssets.ToJavaScriptInputs());
        }
    }
    
    static class JavaScriptAssetExtension
    {
        public static IEnumerable<JavaScriptInput> ToJavaScriptInputs(this IEnumerable<JavaScriptAsset> assets)
        {
            return assets.Select(asset => asset ? JavaScriptInput.FromAsset(asset) : JavaScriptInput.Empty())
                .Where(input => input != null);
        }
    }
}