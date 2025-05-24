using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ItemScriptMergerCompiler
    {
        static readonly JavaScriptGenerator Gen = JavaScriptGenerator.ForItemScript();

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = BuildItemScript(clusterScriptAssetMerger.JavaScriptSource);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildItemScript(JavaScriptSource javaScriptSource)
        {
            return Gen.MergeScripts(javaScriptSource);
        }
    }
}
