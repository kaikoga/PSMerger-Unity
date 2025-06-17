using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClusterVR.CreatorKit.Item;
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

        JavaScriptCompilerEnvironment(JavaScriptSource source, string fileName, string assetPath)
        {
            ScriptLibraries = source.scriptLibraries
                .Where(asset => asset != null)
                .Select(asset => new JavaScriptInput(asset))
                .ToArray();
            ScriptContexts = source.scriptContexts
                .Select(context => new JavaScriptCompilerContext(context))
                .ToArray();
            DetectCallbackSupport = source.detectCallbackSupport;
            OutputFileName = fileName;
            OutputAssetPath = assetPath;
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptComponentMergerBase component)
        {
            var itemName = component.gameObject.GetComponent<IItem>().ItemName ?? component.gameObject.name;
            return new JavaScriptCompilerEnvironment(
                component.JavaScriptSource,
                itemName,
                "");
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptAssetMerger asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset.MergedScript);
            return new JavaScriptCompilerEnvironment(
                asset.JavaScriptSource,
                Path.GetFileName(assetPath),
                assetPath);
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
