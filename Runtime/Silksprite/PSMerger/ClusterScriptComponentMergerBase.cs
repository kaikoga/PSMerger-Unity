using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [RequireComponent(typeof(ScriptableItem)), DisallowMultipleComponent]
    public abstract class ClusterScriptComponentMergerBase : MonoBehaviour
    {
        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] JavaScriptAsset mergedScript;
        [SerializeField] bool generateSourcemap;

        public JavaScriptSource JavaScriptSource => javaScriptSource;
        public JavaScriptAsset MergedScript => mergedScript;
        public bool GenerateSourcemap => generateSourcemap;

        public void SetMergedScript(JavaScriptAsset javaScriptAsset)
        {
            mergedScript = javaScriptAsset;
        }
    }
}
