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

        protected abstract void Compile(T mergerBase);

        void UpdateDisplay()
        {
            _inlineInfoArea.style.display = _mergerBase.JavaScriptSource.HasInlineScriptPlaceholder ? DisplayStyle.Flex : DisplayStyle.None;
            _createInlineJavaScriptButtonArea.style.display = !_mergerBase.TryGetComponent<InlineJavaScript>(out var _) ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
