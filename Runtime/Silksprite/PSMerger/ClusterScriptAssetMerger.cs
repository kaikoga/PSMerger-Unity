using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class ClusterScriptAssetMerger : ScriptableObject
    {
        [SerializeField] ClusterScriptType scriptType = ClusterScriptType.PlayerScript;
        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] JavaScriptAsset mergedScript;
        [SerializeField] bool generateSourcemap;
        
        public ClusterScriptType ScriptType => scriptType;
        public JavaScriptSource JavaScriptSource => javaScriptSource;
        public JavaScriptAsset MergedScript => mergedScript;
        public bool GenerateSourcemap => generateSourcemap;

        public void SetMergedScript(JavaScriptAsset javaScriptAsset)
        {
            mergedScript = javaScriptAsset;
        }
    }

    public enum ClusterScriptType
    {
        ConcatOnly,
        ItemScript,
        PlayerScript
    }
}
