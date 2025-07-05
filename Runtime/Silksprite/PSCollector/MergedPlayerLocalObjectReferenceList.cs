using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.PSCollector.Attributes;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/Merged Player Local Object Reference List")]
    public class MergedPlayerLocalObjectReferenceList : MonoBehaviour, IMergedAccessEntryList<PlayerLocalObjectReferenceListAccessEntry>
    {
        [SerializeField] MergedPlayerLocalObjectReferenceListAccessEntry[] playerLocalObjectReferences;
        
        public IEnumerable<PlayerLocalObjectReferenceListAccessEntry> Entries => playerLocalObjectReferences.Select(entry => new PlayerLocalObjectReferenceListAccessEntry
        {
            id = MergedIdStringBuilder.Build(entry.id, gameObject),
            targetObject = entry.targetObject
        });
    }
    
    [Serializable]
    public sealed class MergedPlayerLocalObjectReferenceListAccessEntry
    {
        [MergedIdString] public string id;
        public GameObject targetObject;

    }
}
