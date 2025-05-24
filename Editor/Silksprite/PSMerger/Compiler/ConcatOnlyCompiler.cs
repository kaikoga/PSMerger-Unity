using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ConcatOnlyCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = ConcatScript(clusterScriptAssetMerger.JavaScriptSource);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string ConcatScript(JavaScriptSource javaScriptSource)
        {
            return string.Join("\n", javaScriptSource.AllScripts);
        }
    }
}
