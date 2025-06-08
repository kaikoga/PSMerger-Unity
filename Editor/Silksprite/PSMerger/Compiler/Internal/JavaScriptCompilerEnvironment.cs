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

        JavaScriptCompilerEnvironment(JavaScriptSource source, string fileName)
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
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptComponentMergerBase component)
        {
            return new JavaScriptCompilerEnvironment(
                component.JavaScriptSource,
                component.gameObject.GetComponent<IItem>().ItemName ?? component.gameObject.name);
        }

        public static JavaScriptCompilerEnvironment Create(ClusterScriptAssetMerger asset)
        {
            return new JavaScriptCompilerEnvironment(
                asset.JavaScriptSource,
                Path.GetFileName(AssetDatabase.GetAssetPath(asset.MergedScript)));
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
