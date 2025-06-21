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

        public readonly string OutputFileName;
        public readonly string OutputAssetPath;

        JavaScriptCompilerEnvironment(JavaScriptSource source, string fileName, string assetPath, string defaultSourceCode)
        {
            ScriptLibraries = source.scriptLibraries
                .ToJavaScriptInputs(fileName, defaultSourceCode)
                .ToArray();
            ScriptContexts = source.scriptContexts
                .Select(context => new JavaScriptCompilerContext(context, fileName, defaultSourceCode))
                .ToArray();
            DetectCallbackSupport = source.detectCallbackSupport;
            OutputFileName = fileName;
            OutputAssetPath = assetPath;
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptComponentMergerBase component)
        {
            var inlineJavaScript = component.gameObject.GetComponent<InlineJavaScript>();
            var itemName = component.gameObject.GetComponent<IItem>().ItemName ?? component.gameObject.name;
            return new JavaScriptCompilerEnvironment(
                component.JavaScriptSource,
                itemName,
                "",
                inlineJavaScript?.SourceCode);
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptAssetMerger asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset.MergedScript);
            return new JavaScriptCompilerEnvironment(
                asset.JavaScriptSource,
                Path.GetFileName(assetPath),
                assetPath,
                null);
        }

        public IEnumerable<JavaScriptInput> AllInputs() => 
            ScriptLibraries.Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs));
    }

    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(JavaScriptContext context, string fileName, string defaultSourceCode)
        {
            JavaScriptInputs = context.JavaScriptAssets
                .ToJavaScriptInputs(fileName, defaultSourceCode)
                .ToArray();
        }
    }

    static class JavaScriptAssetExtension
    {
        public static IEnumerable<JavaScriptInput> ToJavaScriptInputs(this IEnumerable<JavaScriptAsset> assets, string fileName, string defaultSourceCode)
        {
            return assets.Select(asset => asset
                ? new JavaScriptInput(asset)
                : !string.IsNullOrEmpty(defaultSourceCode)
                    ? new JavaScriptInput(fileName, defaultSourceCode)
                    : null)
                .Where(input => input != null);
        }
    }
}
