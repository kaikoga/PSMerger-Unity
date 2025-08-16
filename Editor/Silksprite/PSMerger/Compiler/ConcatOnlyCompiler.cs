using Silksprite.PSMerger.Compiler.Internal;
using Silksprite.PSCore.Access;
using Silksprite.PSMerger.CompilerAPI.Data;
using Silksprite.PSMerger.CompilerAPI.Extensions;
using Silksprite.PSMerger.CompilerAPI.Filter;

namespace Silksprite.PSMerger.Compiler
{
    public static class ConcatOnlyCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = clusterScriptAssetMerger.ToCompilerEnvironment();
            var output = JavaScriptCompilerOutput.CreateFromAssetOutput(clusterScriptAssetMerger.MergedScript);
            ConcatScript(env, output);
            var sourceCode = PSMergerFilter.ApplyPostProcess(output.SourceCode(), clusterScriptAssetMerger);
            javaScriptAssetAccess.text = sourceCode;
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
