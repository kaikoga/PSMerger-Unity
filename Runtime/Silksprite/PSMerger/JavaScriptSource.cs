using System;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [Serializable]
    public class JavaScriptSource
    {
        [SerializeField] internal JavaScriptAsset[] scriptLibraries = { };
        [SerializeField] internal JavaScriptContext[] scriptContexts = { };
     
        public bool HasInlineScriptPlaceholder => 
            scriptLibraries
                .Concat(scriptContexts.SelectMany(context => context.JavaScriptAssets))
                .Any(asset => asset is null);
    }
}
