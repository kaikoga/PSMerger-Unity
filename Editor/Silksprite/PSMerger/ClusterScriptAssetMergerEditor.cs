using System;
using System.IO;
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
        ClusterScriptAssetMerger _merger;

        void OnEnable()
        {
            _merger = (ClusterScriptAssetMerger)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
            content.Add(new IMGUIContainer(() =>
            {
                using (new EditorGUI.DisabledScope(_merger.MergedScript))
                {
                    if (GUILayout.Button("Create Merged Script"))
                    {
                        CreateMergedScript();
                    }
                }
                if (GUILayout.Button("Compile"))
                {
                    Compile();
                }
            }));
            return content;
        }

        void CreateMergedScript()
        {
            var assetPath = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(_merger))}/MergedClusterScript.js";
            var javaScriptAsset = PSMergerUtil.CreateJavaScriptAsset(assetPath);
            _merger.SetMergedScript(javaScriptAsset);
        }

        void Compile()
        {
            switch (_merger.ScriptType)
            {
                case ClusterScriptType.ConcatOnly:
                    ConcatOnlyCompiler.Compile(_merger);
                    break;
                case ClusterScriptType.ItemScript:
                    ItemScriptMergerCompiler.Compile(_merger);
                    break;
                case ClusterScriptType.PlayerScript:
                    PlayerScriptMergerCompiler.Compile(_merger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_merger.ScriptType), _merger.ScriptType, null);
            }
        }
    }
}
