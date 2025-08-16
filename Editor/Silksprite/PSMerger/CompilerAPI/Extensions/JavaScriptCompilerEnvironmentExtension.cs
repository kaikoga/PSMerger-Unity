using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.CompilerAPI.Data;
using Silksprite.PSMerger.CompilerAPI.Filter;
using UnityEditor;

namespace Silksprite.PSMerger.CompilerAPI.Extensions
{
    public static class JavaScriptCompilerEnvironmentExtension
    {
        public static JavaScriptCompilerEnvironment ToCompilerEnvironment(this ClusterScriptComponentMergerBase mergerBase)
        {
            var environment = CreateEnvironment(
                mergerBase.JavaScriptSources().Concat(mergerBase.CollectMergedSources()),
                mergerBase.DetectCallbackSupport);
            return PSMergerFilter.Apply(environment, mergerBase);
        }

        public static JavaScriptCompilerEnvironment ToCompilerEnvironment(this ClusterScriptAssetMerger merger)
        {
            var environment = CreateEnvironment(
                merger.JavaScriptSources(),
                merger.DetectCallbackSupport);
            return PSMergerFilter.Apply(environment, merger);
        }

        static JavaScriptCompilerEnvironment CreateEnvironment(IEnumerable<JavaScriptSource> sources, bool detectCallbackSupport)
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
        
        static IEnumerable<JavaScriptInput> ToJavaScriptInputs(this IEnumerable<JavaScriptAsset> assets)
        {
            return assets.Select(asset => asset ? JavaScriptInputFactory.FromAsset(asset) : JavaScriptInput.Empty())
                .Where(input => input != null);
        }
    }
    
    static class JavaScriptInputFactory
    {
        public static JavaScriptInput FromAsset(JavaScriptAsset asset)
        {
            var sourceCode = asset.text;
            var sourceCodePath = AssetDatabase.GetAssetPath(asset);
            var sourcemapPath = $"{sourceCodePath}.map";
            var sourcemap = File.Exists(sourcemapPath) ? File.ReadAllText(sourcemapPath, System.Text.Encoding.UTF8) : null;
            return new JavaScriptInput(sourceCode, sourceCodePath, sourcemap);
        }
    }
}