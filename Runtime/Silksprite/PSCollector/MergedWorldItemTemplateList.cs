using System.Collections.Generic;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector
{
    [AddComponentMenu("Silksprite/PSCollector/MergedWorldItemTemplateList")]
    public class MergedWorldItemTemplateList : MonoBehaviour, IMergedAccessEntryList<WorldItemTemplateListAccessEntry>
    {
        [SerializeField] WorldItemTemplateListAccessEntry[] worldItemTemplates;
        
        public IEnumerable<WorldItemTemplateListAccessEntry> Entries => worldItemTemplates;
    }
}
