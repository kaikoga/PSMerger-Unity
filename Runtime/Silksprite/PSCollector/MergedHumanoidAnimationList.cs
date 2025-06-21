using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedHumanoidAnimationList : MonoBehaviour, IMergedAccessEntryList<HumanoidAnimationListAccessEntry>
    {
        [SerializeField] HumanoidAnimationListAccessEntry[] humanoidAnimations;
        
        public IEnumerable<HumanoidAnimationListAccessEntry> Entries => humanoidAnimations;
    }
}
