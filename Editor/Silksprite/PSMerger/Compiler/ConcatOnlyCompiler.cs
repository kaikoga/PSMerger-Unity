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
            var output = JavaScriptCompilerOutput.CreateFromAssetOutput(clusterScriptAssetMerger.MergedScript);
            ConcatScript(env, output);
            javaScriptAssetAccess.text = output.SourceCode();
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            }
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static void ConcatScript(JavaScriptCompilerEnvironment env, JavaScriptCompilerOutput output)
        {
            foreach (var script in env.AllInputs())
            {
                output.AppendInput(script);
            }
        }
    }
}
