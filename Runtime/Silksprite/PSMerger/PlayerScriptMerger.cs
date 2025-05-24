using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [RequireComponent(typeof(PlayerScript)), DisallowMultipleComponent]
    public class PlayerScriptMerger : MonoBehaviour
    {
        [SerializeField] JavaScriptSource javaScriptSource;

        public JavaScriptSource JavaScriptSource => javaScriptSource;
    }
}
