using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler
{
    public static class PlayerScriptMergerCompiler
    {
        const string ItemScriptPath = "Packages/net.kaikoga.psmerger/ClusterScripts/GlobalPlayerScriptSetter.js";

        static JavaScriptAsset _itemScriptAsset;
        static JavaScriptAsset ItemScriptAsset => _itemScriptAsset ??= AssetDatabase.LoadAssetAtPath<JavaScriptAsset>(ItemScriptPath);

        static readonly JavaScriptGenerator Gen = JavaScriptGenerator.ForPlayerScript();

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
                playerScriptAccess.sourceCode = BuildPlayerScript(playerScriptMerger.JavaScriptSource);
                changed |= playerScriptAccess.hasModifiedProperties;
            }

            return changed;
        }

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = BuildPlayerScript(clusterScriptAssetMerger.JavaScriptSource);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildPlayerScript(JavaScriptSource javaScriptSource)
        {
            return Gen.MergeScripts(javaScriptSource);
        }
    }
}
