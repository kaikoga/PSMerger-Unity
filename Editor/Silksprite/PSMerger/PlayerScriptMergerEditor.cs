using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(PlayerScriptMerger))]
    public class PlayerScriptMergerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Compile"))
            {
                PlayerScriptMergerCompiler.Compile((PlayerScriptMerger)target);
            }
        }
    }
}
