using System;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [Serializable]
    public class JavaScriptContext
    {
        [SerializeField] JavaScriptAsset[] javaScriptAssets = {};
        
        public JavaScriptAsset[] JavaScriptAssets => javaScriptAssets;
    }
}
