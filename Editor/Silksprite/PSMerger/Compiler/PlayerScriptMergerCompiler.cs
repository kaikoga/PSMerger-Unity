using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
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
            using (var scriptableItemAccess = new ScriptableItemAccess(playerScriptMerger.GetComponent<ScriptableItem>()))
            {
                scriptableItemAccess.sourceCodeAsset = ItemScriptAsset;
                scriptableItemAccess.sourceCode = null;
                changed |= scriptableItemAccess.hasModifiedProperties;
            }

            using (var playerScriptAccess = new PlayerScriptAccess(playerScriptMerger.GetComponent<PlayerScript>()))
            {
                playerScriptAccess.sourceCodeAsset = null;
                var env = JavaScriptCompilerEnvironment.Create(playerScriptMerger);
                var output = BuildPlayerScript(env);
                playerScriptAccess.sourceCode = output.SourceCode();
                changed |= playerScriptAccess.hasModifiedProperties;
            }

            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            var env = JavaScriptCompilerEnvironment.Create(clusterScriptAssetMerger);
            var output = BuildPlayerScript(env);
            javaScriptAssetAccess.text = output.SourceCode();
            if (clusterScriptAssetMerger.GenerateSourcemap)
            {
                javaScriptAssetAccess.sourcemap = output.Sourcemap();
            } 
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static JavaScriptCompilerOutput BuildPlayerScript(JavaScriptCompilerEnvironment env)
        {
            return Gen.MergeScripts(env);
        }
    }
}
