using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged Icon Asset List")]
    public class MergedIconAssetList : MonoBehaviour, IMergedAccessEntryList<IconAssetListAccessEntry>
    {
        [SerializeField] MergedIconAssetListAccessEntry[] iconAssets;
        
        public IEnumerable<IconAssetListAccessEntry> Entries => iconAssets.Select(entry => new IconAssetListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            image = entry.image
        });
    }

    [Serializable]
    public sealed class MergedIconAssetListAccessEntry
    {
        [MergedIdString] public string id;
        public Texture2D image;
    }
}
