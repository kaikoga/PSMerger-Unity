using System;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedPlayerLocalObjectReferenceList : MonoBehaviour
    {
        [SerializeField] MergedPlayerLocalObjectReferenceListEntry[] playerLocalObjectReferences;
        
        public MergedPlayerLocalObjectReferenceListEntry[] PlayerLocalObjectReferences => playerLocalObjectReferences;
        
        [Serializable]
        public class MergedPlayerLocalObjectReferenceListEntry
        {
            public string id;
            public GameObject targetObject;
        }
    }
}
