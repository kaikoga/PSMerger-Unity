using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedIconAssetList : MonoBehaviour, IMergedAccessEntryList<IconAssetListAccessEntry>
    {
        [SerializeField] IconAssetListAccessEntry[] iconAssets;
        
        public IEnumerable<IconAssetListAccessEntry> Entries => iconAssets;
    }
}
