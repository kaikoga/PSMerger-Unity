using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged Icon Asset List")]
    public class MergedIconAssetList : MonoBehaviour, IMergedAccessEntryList<IconAssetListAccessEntry>
    {
        [SerializeField] IconAssetListAccessEntry[] iconAssets;
        
        public IEnumerable<IconAssetListAccessEntry> Entries => iconAssets;
    }
}
