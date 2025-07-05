using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged Item Audio Set List")]
    public class MergedItemAudioSetList : MonoBehaviour, IMergedAccessEntryList<ItemAudioSetListAccessEntry>
    {
        [SerializeField] MergedItemAudioSetListAccessEntry[] itemAudioSets;
        
        public IEnumerable<ItemAudioSetListAccessEntry> Entries => itemAudioSets.Select(entry => new ItemAudioSetListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            audioClip = entry.audioClip,
            loop = entry.loop
        });
    }
    
    [Serializable]
    public sealed class MergedItemAudioSetListAccessEntry
    {
        [MergedIdString] public string id;
        public AudioClip audioClip;
        public bool loop;
    }
}
