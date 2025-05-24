using System;
using System.Collections.Generic;
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
        
        public JavaScriptAsset[] ScriptLibraries => scriptLibraries;
        public JavaScriptAsset[][] ScriptContexts => scriptContexts.Select(context => context.JavaScriptAssets).ToArray();

        public JavaScriptAsset[] AllScripts => 
            scriptLibraries
                .Concat(ScriptContexts.SelectMany(asset => asset))
                .ToArray();

        public JavaScriptSource()
        {
        }

        public JavaScriptSource(JavaScriptAsset[] scriptLibraries, JavaScriptContext[] scriptContexts)
        {
            this.scriptLibraries = scriptLibraries;
            this.scriptContexts = scriptContexts;
        }
    }
}
