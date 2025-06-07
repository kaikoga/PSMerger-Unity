using Silksprite.PSMerger.Access;
using Silksprite.PSMerger.Compiler.Internal;

namespace Silksprite.PSMerger.Compiler
{
    public static class ConcatOnlyCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var output = ConcatScript(clusterScriptAssetMerger.JavaScriptSource);
            javaScriptAssetAccess.text = output.SourceCode();
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static JavaScriptCompilerOutput ConcatScript(JavaScriptSource javaScriptSource)
        {
            var env = new JavaScriptCompilerEnvironment(javaScriptSource);
            var output = new JavaScriptCompilerOutput();
            foreach (var script in env.AllScripts)
            {
                output.AppendLines(script);
            }
            return output;
        }
    }
}
