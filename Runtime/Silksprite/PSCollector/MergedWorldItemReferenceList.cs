using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedWorldItemReferenceList : MonoBehaviour, IMergedAccessEntryList<WorldItemReferenceListAccessEntry>
    {
        [SerializeField] WorldItemReferenceListAccessEntry[] worldItemReferences;
        
        public IEnumerable<WorldItemReferenceListAccessEntry> Entries => worldItemReferences;
    }
}
