using System.IO;
using ClusterVR.CreatorKit.Editor.Custom;
using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Silksprite.PSMerger.ClusterScriptAssetMerger;

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
            var container = new VisualElement();

            container.Add(new PropertyField(serializedObject.FindProperty(NameofJavaScriptSource)));
            container.Add(new PropertyField(serializedObject.FindProperty(NameofOtherSources)));
            container.Add(new PropertyField(serializedObject.FindProperty(NameofMergedScript)));
            container.Add(new IMGUIContainer(() =>
            {
                using (new EditorGUI.DisabledScope(_merger.MergedScript))
                {
                    if (GUILayout.Button("Create Merged Script"))
                    {
                        CreateMergedScript();
                    }
                }
            }));

            var advanced = new Foldout
            {
                text = "上級者向け設定",
            };
            advanced.Add(new PropertyField(serializedObject.FindProperty(NameofDetectCallbackSupport)));
            advanced.Add(new HelpBox
            {
                text = "オンの場合、コールバックのサポートを必要に応じて生成します。",
                messageType = HelpBoxMessageType.Info
            });
            advanced.Add(new PropertyField(serializedObject.FindProperty(NameofGenerateSourcemap)));
            container.Add(advanced);

            container.Bind(serializedObject);

            container.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Compile"))
                {
                    ClusterScriptAssetMergerCompiler.Compile(_merger);
                }
            }));
            return container;
        }

        void CreateMergedScript()
        {
            var assetPath = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(_merger))}/MergedClusterScript.js";
            var javaScriptAsset = PSMergerUtil.CreateJavaScriptAsset(assetPath);
            _merger.SetMergedScript(javaScriptAsset);
        }

    }
}
