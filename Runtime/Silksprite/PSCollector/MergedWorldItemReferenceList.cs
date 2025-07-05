using System;
using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged World Item Reference List")]
    public class MergedWorldItemReferenceList : MonoBehaviour, IMergedAccessEntryList<WorldItemReferenceListAccessEntry>
    {
        [SerializeField] MergedWorldItemReferenceListAccessEntry[] worldItemReferences;
        
        public IEnumerable<WorldItemReferenceListAccessEntry> Entries => worldItemReferences.Select(entry => new WorldItemReferenceListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            item = entry.item
        });
    }
    
    [Serializable]
    public sealed class MergedWorldItemReferenceListAccessEntry
    {
        [MergedIdString] public string id;
        public Item item;

    }
}
