using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(ItemScriptMerger))]
    public class ItemScriptMergerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Compile"))
            {
                ItemScriptMergerCompiler.Compile((ItemScriptMerger)target);
            }
        }
    }
}
