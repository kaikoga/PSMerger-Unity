using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [AddComponentMenu("Silksprite/PSMerger/ItemScript Merger", 100)]
    [RequireComponent(typeof(ScriptableItem))]
    public class ItemScriptMerger : ClusterScriptComponentMergerBase<IMergedItemScriptSource>
    {
    }
}
