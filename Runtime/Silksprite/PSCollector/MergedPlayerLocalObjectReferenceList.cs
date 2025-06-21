using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedPlayerLocalObjectReferenceList : MonoBehaviour
    {
        [SerializeField] PlayerLocalObjectReferenceListAccessEntry[] playerLocalObjectReferences;
        
        public PlayerLocalObjectReferenceListAccessEntry[] PlayerLocalObjectReferences => playerLocalObjectReferences;
    }
}
