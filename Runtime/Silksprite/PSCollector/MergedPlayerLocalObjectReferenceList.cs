using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/MergedPlayerLocalObjectReferenceList")]
    public class MergedPlayerLocalObjectReferenceList : MonoBehaviour, IMergedAccessEntryList<PlayerLocalObjectReferenceListAccessEntry>
    {
        [SerializeField] PlayerLocalObjectReferenceListAccessEntry[] playerLocalObjectReferences;
        
        public IEnumerable<PlayerLocalObjectReferenceListAccessEntry> Entries => playerLocalObjectReferences;
    }
}
