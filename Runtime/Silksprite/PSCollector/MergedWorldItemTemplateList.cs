using System;
using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged World Item Template List")]
    public class MergedWorldItemTemplateList : MonoBehaviour, IMergedAccessEntryList<WorldItemTemplateListAccessEntry>
    {
        [SerializeField] MergedWorldItemTemplateListAccessEntry[] worldItemTemplates;
        
        public IEnumerable<WorldItemTemplateListAccessEntry> Entries => worldItemTemplates.Select(entry => new WorldItemTemplateListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            worldItemTemplate = entry.worldItemTemplate
        });
    }
    
    [Serializable]
    public sealed class MergedWorldItemTemplateListAccessEntry
    {
        [MergedIdString] public string id;
        public Item worldItemTemplate;
        
        public WorldItemTemplateListAccessEntry ToAccessEntry()
        {
            return new WorldItemTemplateListAccessEntry
            {
                id = id,
                worldItemTemplate = worldItemTemplate
            };
        }
    }
}
