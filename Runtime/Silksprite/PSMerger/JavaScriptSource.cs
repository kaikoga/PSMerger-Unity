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
        
        public string[] ScriptLibraries => scriptLibraries.FilterTexts().ToArray();
        public string[][] ScriptContexts => scriptContexts.Select(context => context.JavaScriptAssets.FilterTexts().ToArray()).ToArray();

        public string[] AllScripts => 
            scriptLibraries.FilterTexts()
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

    static class EnumerableJavaScriptAssetExtensions
    {
        public static IEnumerable<string> FilterTexts(this IEnumerable<JavaScriptAsset> assets)
        {
            return assets
                .Select(asset => asset ? asset.text : null)
                .Where(text => text is not null);
        }
    }
}
