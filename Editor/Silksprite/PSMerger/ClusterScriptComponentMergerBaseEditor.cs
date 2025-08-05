using System.IO;
using System.Linq;
using ClusterVR.CreatorKit.Editor.Custom;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Silksprite.PSMerger.ClusterScriptComponentMergerBase;

namespace Silksprite.PSMerger
{
    public abstract class ClusterScriptComponentMergerBaseEditor<T> : VisualElementEditor
    where T : ClusterScriptComponentMergerBase
    {
        T _mergerBase;
        VisualElement _inlineInfoArea;
        IMGUIContainer _createInlineJavaScriptButtonArea;
        PropertyField _generateSourceMap;

        void OnEnable()
        {
            _mergerBase = (T)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            container.Add(new PropertyField(serializedObject.FindProperty(NameofJavaScriptSource)));
            container.Add(new PropertyField(serializedObject.FindProperty(NameofOtherSources)));

            var advanced = new Foldout
            {
                text = "上級者向け設定",
            };
            advanced.Add(new PropertyField(serializedObject.FindProperty(NameofMergedScript)));
            advanced.Add(new IMGUIContainer(() =>
            {
                using (new EditorGUI.DisabledScope(_mergerBase.MergedScript))
                {
                    if (GUILayout.Button("Create Merged Script"))
                    {
                        CreateMergedScript();
                    }
                }
            }));
            _generateSourceMap = new PropertyField(serializedObject.FindProperty(NameofGenerateSourcemap));
            advanced.Add(_generateSourceMap);
            advanced.Add(new PropertyField(serializedObject.FindProperty(NameofDetectCallbackSupport)));
            advanced.Add(new HelpBox
            {
                text = "オンの場合、コールバックのサポートを必要に応じて生成します。",
                messageType = HelpBoxMessageType.Info
            });
            container.Add(advanced);

            container.Bind(serializedObject);

            _inlineInfoArea = new VisualElement
            {
                style =
                {
                    marginBottom = EditorGUIUtility.singleLineHeight
                }
            };
            _inlineInfoArea.Add(new HelpBox("InlineJavaScriptコンポーネントがこのオブジェクトについている場合、Noneのスロットに内容が入ります。", HelpBoxMessageType.Info));
            _createInlineJavaScriptButtonArea = new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Create InlineJavaScript"))
                {
                    Undo.AddComponent<InlineJavaScript>(_mergerBase.gameObject);
                    UpdateDisplay();
                }
            });
            _inlineInfoArea.Add(_createInlineJavaScriptButtonArea);
            container.Add(_inlineInfoArea);
            
            container.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Compile"))
                {
                    Compile(_mergerBase);
                }
            }));

            container.TrackSerializedObjectValue(serializedObject, _ => UpdateDisplay());
            UpdateDisplay();

            return container;
        }

        void CreateMergedScript()
        {
            var assetPath = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(_mergerBase))}/MergedClusterScript.js";
            var javaScriptAsset = PSMergerUtil.CreateJavaScriptAsset(assetPath);
            _mergerBase.SetMergedScript(javaScriptAsset);
        }

        protected abstract void Compile(T mergerBase);

        void UpdateDisplay()
        {
            _generateSourceMap.style.display = _mergerBase.MergedScript ? DisplayStyle.Flex : DisplayStyle.None;
            _inlineInfoArea.style.display = _mergerBase.JavaScriptSources().Any(source => source.HasInlineScriptPlaceholder) ? DisplayStyle.Flex : DisplayStyle.None;
            _createInlineJavaScriptButtonArea.style.display = !_mergerBase.TryGetComponent<InlineJavaScript>(out var _) ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
