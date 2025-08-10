using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.Compiler.Filter;
using Silksprite.PSMerger.Compiler.Internal;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler
{
    public static class PlayerScriptMergerCompiler
    {
        const string ItemScriptPath = "Packages/net.kaikoga.psmerger/ClusterScripts/GlobalPlayerScriptSetter.js";

        static JavaScriptAsset _itemScriptAsset;
        static JavaScriptAsset ItemScriptAsset => _itemScriptAsset ??= AssetDatabase.LoadAssetAtPath<JavaScriptAsset>(ItemScriptPath);

        static readonly MergedJavaScriptGenerator Gen = MergedJavaScriptGenerator.ForPlayerScript();

        public static bool Compile(PlayerScriptMerger playerScriptMerger)
        {
            var changed = false;
            if (playerScriptMerger.UseGlobalPlayerScriptSetter && !playerScriptMerger.TryGetComponent<ItemScriptMerger>(out _))
            {
                using var scriptableItemAccess = new ScriptableItemAccess(playerScriptMerger.GetComponent<ScriptableItem>());
                scriptableItemAccess.sourceCodeAsset = ItemScriptAsset;
                scriptableItemAccess.sourceCode = null;
                changed |= scriptableItemAccess.hasModifiedProperties;
            }

            using (var playerScriptAccess = new PlayerScriptAccess(playerScriptMerger.GetComponent<PlayerScript>()))
            {
                var env = playerScriptMerger.ToCompilerEnvironment();
                var output = JavaScriptCompilerOutput.CreateFromAssetOutput(playerScriptMerger.MergedScript);
                BuildPlayerScript(env, output);
                var sourceCode = PSMergerFilter.ApplyPostProcess(output.SourceCode(), playerScriptMerger);
                if (playerScriptMerger.MergedScript)
                {
                    playerScriptAccess.sourceCodeAsset = playerScriptMerger.MergedScript;
                    using var javaScriptAssetAccess = new JavaScriptAssetAccess(playerScriptMerger.MergedScript);
                    javaScriptAssetAccess.text = sourceCode;
                    if (playerScriptMerger.GenerateSourcemap)
                    {
                        javaScriptAssetAccess.sourcemap = output.Sourcemap();
                    } 
                    changed |= javaScriptAssetAccess.hasModifiedProperties;
                }
                else
                {
                    playerScriptAccess.sourceCodeAsset = null;
                    playerScriptAccess.sourceCode = sourceCode;
                }
                changed |= playerScriptAccess.hasModifiedProperties;
            }

            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = clusterScriptAssetMerger.ToCompilerEnvironment();
            var output = JavaScriptCompilerOutput.CreateFromAssetOutput(clusterScriptAssetMerger.MergedScript);
            BuildPlayerScript(env, output);
            var sourceCode = PSMergerFilter.ApplyPostProcess(output.SourceCode(), clusterScriptAssetMerger);
            javaScriptAssetAccess.text = sourceCode;
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            } 
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static void BuildPlayerScript(JavaScriptCompilerEnvironment env, JavaScriptCompilerOutput output)
        {
            Gen.MergeScripts(env, output);
        }
    }
}
