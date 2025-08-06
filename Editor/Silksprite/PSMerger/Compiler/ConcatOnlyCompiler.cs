using Silksprite.PSMerger.Compiler.Internal;
using Silksprite.PSCore.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ConcatOnlyCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = JavaScriptCompilerEnvironment.Create(clusterScriptAssetMerger);
            var output = ConcatScript(env);
            javaScriptAssetAccess.text = output.SourceCode();
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            }
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static JavaScriptCompilerOutput ConcatScript(JavaScriptCompilerEnvironment env)
        {
            var output = env.Output;
            foreach (var script in env.AllInputs())
            {
                output.AppendInput(script);
            }
            return output;
        }
    }
}
