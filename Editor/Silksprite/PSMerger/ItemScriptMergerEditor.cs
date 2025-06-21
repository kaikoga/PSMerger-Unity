using Silksprite.PSMerger.Compiler;
using UnityEditor;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(ItemScriptMerger))]
    public class ItemScriptMergerEditor : ClusterScriptComponentMergerBaseEditor<ItemScriptMerger>
    {
        protected override void Compile(ItemScriptMerger mergerBase)
        {
            ItemScriptMergerCompiler.Compile(mergerBase);
        }
    }
}
