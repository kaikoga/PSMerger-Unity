using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged Humanoid Animation List")]
    public class MergedHumanoidAnimationList : MonoBehaviour, IMergedAccessEntryList<HumanoidAnimationListAccessEntry>
    {
        [SerializeField] MergedHumanoidAnimationListAccessEntry[] humanoidAnimations;
        
        public IEnumerable<HumanoidAnimationListAccessEntry> Entries => humanoidAnimations.Select(entry => new HumanoidAnimationListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            animation = entry.animation
        });
    }
    
    [Serializable]
    public sealed class MergedHumanoidAnimationListAccessEntry
    {
        [MergedIdString] public string id;
        public AnimationClip animation;
    }
}
