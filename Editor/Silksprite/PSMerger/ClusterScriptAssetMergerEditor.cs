using System;
using ClusterVR.CreatorKit.Editor.Custom;
using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger
{
    [CustomEditor(typeof(ClusterScriptAssetMerger))]
    public class ClusterScriptAssetMergerEditor : VisualElementEditor
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
            var merger = (ClusterScriptAssetMerger)target;
            switch (merger.ScriptType)
            {
                case ClusterScriptType.ItemScript:
                    ItemScriptMergerCompiler.Compile(merger);
                    break;
                case ClusterScriptType.PlayerScript:
                    PlayerScriptMergerCompiler.Compile(merger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(merger.ScriptType), merger.ScriptType, null);
            }
        }
    }
}
