using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class ClusterScriptAssetMerger : ScriptableObject
    {
        [SerializeField] ClusterScriptType scriptType = ClusterScriptType.PlayerScript;
        [SerializeField] JavaScriptContext[] scriptContexts = {};
        [SerializeField] JavaScriptAsset mergedScript;
        
        public ClusterScriptType ScriptType => scriptType;
        public JavaScriptAsset[][] ScriptContexts => scriptContexts.Select(context => context.JavaScriptAssets).ToArray();
        public JavaScriptAsset MergedScript => mergedScript;

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
