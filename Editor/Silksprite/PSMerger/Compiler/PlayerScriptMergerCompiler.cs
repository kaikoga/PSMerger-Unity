using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler
{
    public static class PlayerScriptMergerCompiler
    {
        const string ItemScriptPath = "Packages/net.net.kaikoga.psmerger/ClusterScripts/GlobalPlayerScriptSetter.js";

        static string _itemScript;
        static string ItemScript => _itemScript ??= AssetDatabase.LoadAssetAtPath<JavaScriptAsset>(ItemScriptPath).text;

        static readonly JavaScriptGenerator Gen = JavaScriptGenerator.ForPlayerScript();

        public static bool Compile(PlayerScriptMerger playerScriptMerger)
        {
            var changed = false;
            using (var scriptableItemAccess = new ScriptableItemAccess(playerScriptMerger.GetComponent<ScriptableItem>()))
            {
                scriptableItemAccess.sourceCodeAsset = null;
                scriptableItemAccess.sourceCode = ItemScript;
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
