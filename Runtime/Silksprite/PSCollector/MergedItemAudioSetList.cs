using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/MergedItemAudioSetList")]
    public class MergedItemAudioSetList : MonoBehaviour, IMergedAccessEntryList<ItemAudioSetListAccessEntry>
    {
        [SerializeField] ItemAudioSetListAccessEntry[] itemAudioSets;
        
        public IEnumerable<ItemAudioSetListAccessEntry> Entries => itemAudioSets;
    }
}
