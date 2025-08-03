using System.IO;
using System.Linq;
using ClusterVR.CreatorKit.Editor.Custom;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger
{
    public abstract class ClusterScriptComponentMergerBaseEditor<T> : VisualElementEditor
    where T : ClusterScriptComponentMergerBase
    {
        T _mergerBase;
        VisualElement _inlineInfoArea;
        IMGUIContainer _createInlineJavaScriptButtonArea;
        
        void OnEnable()
        {
            _mergerBase = (T)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
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
            content.Add(_inlineInfoArea);
            
            content.Add(new IMGUIContainer(() =>
            {
                using (new EditorGUI.DisabledScope(_mergerBase.MergedScript))
                {
                    if (GUILayout.Button("Create Merged Script"))
                    {
                        CreateMergedScript();
                    }
                }
                if (GUILayout.Button("Compile"))
                {
                    Compile(_mergerBase);
                }
            }));

            content.TrackSerializedObjectValue(serializedObject, _ =>
            {
                UpdateDisplay();
            });
            UpdateDisplay();

            return content;
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
            _inlineInfoArea.style.display = _mergerBase.JavaScriptSources().Any(source => source.HasInlineScriptPlaceholder) ? DisplayStyle.Flex : DisplayStyle.None;
            _createInlineJavaScriptButtonArea.style.display = !_mergerBase.TryGetComponent<InlineJavaScript>(out var _) ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
