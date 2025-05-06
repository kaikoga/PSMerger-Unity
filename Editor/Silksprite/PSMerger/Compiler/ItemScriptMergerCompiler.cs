using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ItemScriptMergerCompiler
    {
        static readonly JavaScriptGenerator Gen = new(false);

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = BuildItemScript(clusterScriptAssetMerger.ScriptContexts);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildItemScript(JavaScriptAsset[][] itemScripts)
        {
            return Gen.MergeScripts(itemScripts);
        }
    }
}
