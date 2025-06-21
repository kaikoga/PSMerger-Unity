using Silksprite.PSMerger.Compiler;
using UnityEditor;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(PlayerScriptMerger))]
    public class PlayerScriptMergerEditor : ClusterScriptComponentMergerBaseEditor<PlayerScriptMerger>
    {
        protected override void Compile(PlayerScriptMerger mergerBase)
        {
            PlayerScriptMergerCompiler.Compile(mergerBase);
        }

    }
}
