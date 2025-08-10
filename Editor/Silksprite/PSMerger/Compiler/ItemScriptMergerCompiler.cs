using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.Compiler.Filter;
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
                var env = itemScriptMerger.ToCompilerEnvironment(Enumerable.Empty<JavaScriptSource>());
                var output = JavaScriptCompilerOutput.CreateFromAssetOutput(itemScriptMerger.MergedScript);
                BuildItemScript(env, output);
                var sourceCode = PSMergerFilter.ApplyPostProcess(output.SourceCode(), itemScriptMerger);
                if (itemScriptMerger.MergedScript)
                {
                    scriptableItemAccess.sourceCodeAsset = itemScriptMerger.MergedScript;
                    using var javaScriptAssetAccess = new JavaScriptAssetAccess(itemScriptMerger.MergedScript);
                    javaScriptAssetAccess.text = sourceCode;
                    if (itemScriptMerger.GenerateSourcemap)
                    {
                        javaScriptAssetAccess.sourcemap = output.Sourcemap();
                    } 
                    changed |= javaScriptAssetAccess.hasModifiedProperties;
                }
                else
                {
                    scriptableItemAccess.sourceCodeAsset = null;
                    scriptableItemAccess.sourceCode = sourceCode;
                }
                changed |= scriptableItemAccess.hasModifiedProperties;
            }
            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = clusterScriptAssetMerger.ToCompilerEnvironment();
            var output = JavaScriptCompilerOutput.CreateFromAssetOutput(clusterScriptAssetMerger.MergedScript);
            BuildItemScript(env, output);
            var sourceCode = PSMergerFilter.ApplyPostProcess(output.SourceCode(), clusterScriptAssetMerger);
            javaScriptAssetAccess.text = sourceCode;
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            } 
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static void BuildItemScript(JavaScriptCompilerEnvironment env, JavaScriptCompilerOutput output)
        {
            Gen.MergeScripts(env, output);
        }
    }
}
