using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedWorldItemReferenceList : MonoBehaviour
    {
        [SerializeField] WorldItemReferenceListAccessEntry[] worldItemReferences;
        
        public WorldItemReferenceListAccessEntry[] WorldItemReferences => worldItemReferences;
    }
}
