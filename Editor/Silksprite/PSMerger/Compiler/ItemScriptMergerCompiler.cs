using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;
using Silksprite.PSMerger.Compiler.Internal;
using UnityEngine;

namespace Silksprite.PSMerger.Compiler
{
    public static class ItemScriptMergerCompiler
    {
        static readonly MergedJavaScriptGenerator Gen = MergedJavaScriptGenerator.ForItemScript();

        public static bool Compile(ItemScriptMerger itemScriptMerger)
        {
            var changed = false;
            using (var scriptableItemAccess = new ScriptableItemAccess(itemScriptMerger.GetComponent<ScriptableItem>()))
            {
                scriptableItemAccess.sourceCodeAsset = null;
                var env = JavaScriptCompilerEnvironment.Create(itemScriptMerger);
                var output = BuildItemScript(env);
                scriptableItemAccess.sourceCode = output.SourceCode();
                changed |= scriptableItemAccess.hasModifiedProperties;
            }
            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = JavaScriptCompilerEnvironment.Create(clusterScriptAssetMerger);
            var output = BuildItemScript(env);
            javaScriptAssetAccess.text = output.SourceCode();
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            } 
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static JavaScriptCompilerOutput BuildItemScript(JavaScriptCompilerEnvironment env)
        {
            return Gen.MergeScripts(env);
        }
    }
}
