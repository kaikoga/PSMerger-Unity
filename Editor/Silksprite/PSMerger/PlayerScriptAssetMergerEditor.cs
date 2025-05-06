using ClusterVR.CreatorKit.Editor.Custom;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(PlayerScriptAssetMerger))]
    public class PlayerScriptAssetMergerEditor : VisualElementEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
            content.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Compile"))
                {
                    Compile();
                }
            }));
            return content;
        }

        void Compile()
        {
            PlayerScriptMergerCompiler.Compile((PlayerScriptAssetMerger)target);
        }
    }
}
