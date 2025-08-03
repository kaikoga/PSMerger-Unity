using System.Collections.Generic;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [RequireComponent(typeof(ScriptableItem)), DisallowMultipleComponent]
    public abstract class ClusterScriptComponentMergerBase : MonoBehaviour
    {
        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] ScriptMergerSource[] otherSources = { };
        [SerializeField] JavaScriptAsset mergedScript;
        [SerializeField] bool generateSourcemap;

        public JavaScriptAsset MergedScript => mergedScript;
        public bool GenerateSourcemap => generateSourcemap;

        public IEnumerable<JavaScriptSource> JavaScriptSources()
        {
            yield return javaScriptSource;
            foreach (var otherSource in otherSources) yield return otherSource?.JavaScriptSource;
        }

        public void SetMergedScript(JavaScriptAsset javaScriptAsset)
        {
            mergedScript = javaScriptAsset;
        }
    }
}
