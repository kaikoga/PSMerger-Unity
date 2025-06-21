using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    public class MergedWorldItemTemplateList : MonoBehaviour
    {
        [SerializeField] WorldItemTemplateListAccessEntry[] worldItemTemplates;
        
        public WorldItemTemplateListAccessEntry[] WorldItemTemplates => worldItemTemplates;
    }
}
