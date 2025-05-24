using ClusterVR.CreatorKit.Editor.Custom;
using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(PlayerScriptMerger))]
    public class PlayerScriptMergerEditor : VisualElementEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
            content.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Compile"))
                {
                    PlayerScriptMergerCompiler.Compile((PlayerScriptMerger)target);
                }
            }));
            return content;
        }
    }
}
