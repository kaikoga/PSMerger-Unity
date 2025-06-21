using System;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedWorldItemTemplateList : MonoBehaviour
    {
        [SerializeField] MergedWorldItemTemplateListEntry[] worldItemTemplates;
        
        public MergedWorldItemTemplateListEntry[] WorldItemTemplates => worldItemTemplates;
        
        [Serializable]
        public class MergedWorldItemTemplateListEntry
        {
            public string id;
            public Item worldItemTemplate;
        }
    }
}
