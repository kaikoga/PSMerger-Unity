using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [RequireComponent(typeof(ScriptableItem)), DisallowMultipleComponent]
    public abstract class ClusterScriptComponentMergerBase : MonoBehaviour
    {
        [SerializeField] JavaScriptSource javaScriptSource;

        public JavaScriptSource JavaScriptSource => javaScriptSource;
    }
}
