using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [AddComponentMenu("Silksprite/PSMerger/PlayerScript Merger", 101)]
    [RequireComponent(typeof(PlayerScript)), DisallowMultipleComponent]
    public class PlayerScriptMerger : ClusterScriptComponentMergerBase<IMergedPlayerScriptSource>
    {
    }
}
