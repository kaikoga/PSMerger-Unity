using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ConcatOnlyCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = ConcatScript(clusterScriptAssetMerger.ScriptContexts);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string ConcatScript(JavaScriptAsset[][] itemScripts)
        {
            return string.Join("\n", itemScripts
                .SelectMany(assets => assets)
                .Select(asset => asset != null ? asset.text : null));
        }
    }
}
