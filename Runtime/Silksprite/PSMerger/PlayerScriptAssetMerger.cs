using System;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class PlayerScriptAssetMerger : ScriptableObject
    {
        [SerializeField] JavaScriptContext[] playerScriptContexts = {};
        
        [SerializeField] JavaScriptAsset mergedPlayerScript;

        public JavaScriptAsset[][] PlayerScripts => playerScriptContexts.Select(context => context.JavaScriptAssets).ToArray();
        public JavaScriptAsset MergedPlayerScript => mergedPlayerScript;
    }

    [Serializable]
    public class JavaScriptContext
    {
        [SerializeField] JavaScriptAsset[] javaScriptAssets = {};
        
        public JavaScriptAsset[] JavaScriptAssets => javaScriptAssets;
    }
}
