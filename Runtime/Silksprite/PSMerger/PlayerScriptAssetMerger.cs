using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class PlayerScriptAssetMerger : ScriptableObject
    {
        [SerializeField] JavaScriptAsset[] playerScripts = {};
        [SerializeField] JavaScriptAsset mergedPlayerScript;

        public JavaScriptAsset[] PlayerScripts => playerScripts;
        public JavaScriptAsset MergedPlayerScript => mergedPlayerScript;
    }
}
