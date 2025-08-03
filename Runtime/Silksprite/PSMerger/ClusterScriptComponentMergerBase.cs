using System.Collections.Generic;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [RequireComponent(typeof(ScriptableItem)), DisallowMultipleComponent]
    public abstract class ClusterScriptComponentMergerBase : MonoBehaviour
    {
        public const string NameofJavaScriptSource = nameof(javaScriptSource);
        public const string NameofOtherSources = nameof(otherSources);
        public const string NameofMergedScript = nameof(mergedScript);
        public const string NameofDetectCallbackSupport = nameof(detectCallbackSupport);
        public const string NameofGenerateSourcemap = nameof(generateSourcemap);

        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] ScriptMergerSource[] otherSources = { };
        [SerializeField] JavaScriptAsset mergedScript;
        [SerializeField] bool detectCallbackSupport = true;
        [SerializeField] bool generateSourcemap;

        public JavaScriptAsset MergedScript => mergedScript;
        public bool DetectCallbackSupport => detectCallbackSupport;
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
