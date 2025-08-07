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
        public const string NameofScriptLibraries = nameof(scriptLibraries);
        public const string NameofScriptContexts = nameof(scriptContexts);
        
        [SerializeField] JavaScriptAsset[] scriptLibraries = { };
        [SerializeField] JavaScriptContext[] scriptContexts = { };

        public IEnumerable<JavaScriptAsset> ScriptLibraries => scriptLibraries;
        public IEnumerable<JavaScriptContext> ScriptContexts => scriptContexts;

        public bool HasInlineScriptPlaceholder => 
            scriptLibraries
                .Concat(scriptContexts.SelectMany(context => context.JavaScriptAssets))
                .Any(asset => asset is null);
    }
}
