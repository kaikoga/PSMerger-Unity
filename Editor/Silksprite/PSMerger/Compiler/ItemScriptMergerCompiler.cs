using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;
using Silksprite.PSMerger.Compiler.Internal;

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
                scriptableItemAccess.sourceCode = BuildItemScript(itemScriptMerger.JavaScriptSource);
                changed |= scriptableItemAccess.hasModifiedProperties;
            }
            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = BuildItemScript(clusterScriptAssetMerger.JavaScriptSource);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildItemScript(JavaScriptSource javaScriptSource)
        {
            return Gen.MergeScripts(new JavaScriptCompilerEnvironment(javaScriptSource));
        }
    }
}
