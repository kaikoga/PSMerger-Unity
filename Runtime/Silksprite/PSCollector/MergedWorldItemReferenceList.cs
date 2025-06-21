using System;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedWorldItemReferenceList : MonoBehaviour
    {
        [SerializeField] MergedWorldItemReferenceListEntry[] worldItemReferences;
        
        public MergedWorldItemReferenceListEntry[] WorldItemReferences => worldItemReferences;
        
        [Serializable]
        public class MergedWorldItemReferenceListEntry
        {
            public string id;
            public Item item;
        }
    }
}
