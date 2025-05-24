using ClusterVR.CreatorKit.Editor.Custom;
using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(ItemScriptMerger))]
    public class ItemScriptMergerEditor : VisualElementEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
            content.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Compile"))
                {
                    ItemScriptMergerCompiler.Compile((ItemScriptMerger)target);
                }
            }));
            return content;
        }
    }
}
