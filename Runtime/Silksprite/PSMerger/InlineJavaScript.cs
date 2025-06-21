using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [AddComponentMenu("Silksprite/PSMerger/Inline JavaScript")]
    [RequireComponent(typeof(ScriptableItem))]
    public class InlineJavaScript : MonoBehaviour
    {
        [SerializeField, TextArea(5, 15)] string sourceCode;
        
        public string SourceCode => sourceCode;
    }
}
