
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(PlayerScriptAssetMerger))]
    public class PlayerScriptAssetMergerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Compile"))
            {
                PlayerScriptMergerCompiler.Compile((PlayerScriptAssetMerger)target);
            }
        }
    }
}
