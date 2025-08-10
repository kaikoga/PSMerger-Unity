using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public abstract class MergedClusterScriptSourceBase : MonoBehaviour, IMergedClusterScriptSourceBase
    {
        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] ScriptMergerSource[] otherSources = { };
        
        public IEnumerable<JavaScriptSource> JavaScriptSources()
        {
            yield return javaScriptSource;
            foreach (var otherSource in otherSources) yield return otherSource?.JavaScriptSource;
        }
    }
}
