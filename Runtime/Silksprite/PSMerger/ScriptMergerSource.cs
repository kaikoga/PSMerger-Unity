using UnityEngine;

namespace Silksprite.PSMerger
{
    public class ScriptMergerSource : ScriptableObject
    {
        [SerializeField] JavaScriptSource javaScriptSource;
        
        public JavaScriptSource JavaScriptSource => javaScriptSource;
    }
}
