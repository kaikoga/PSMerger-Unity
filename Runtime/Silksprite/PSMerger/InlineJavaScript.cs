using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [AddComponentMenu("Silksprite/PSMerger/Inline JavaScript", 200)]
    [RequireComponent(typeof(ScriptableItem))]
    public class InlineJavaScript : MonoBehaviour, IInlineJavaScript
    {
        [SerializeField, TextArea(5, 15)] string sourceCode;
        
        public string SourceCode => sourceCode;
    }
}
