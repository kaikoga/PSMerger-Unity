using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(JavaScriptSource))]
    public class JavaScriptSourceDrawer : PropertyDrawer
    {
        static bool _advanced = false;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var scriptLibraries = property.FindPropertyRelative(nameof(JavaScriptSource.scriptLibraries));
            var scriptContexts = property.FindPropertyRelative(nameof(JavaScriptSource.scriptContexts));
            var detectCallbackSupport = property.FindPropertyRelative(nameof(JavaScriptSource.detectCallbackSupport));

            var container = new VisualElement();
            container.Add(new PropertyField(scriptLibraries)
            {
                name = "PropertyField:" + scriptLibraries.propertyPath
            });
            container.Add(new HelpBox
            {
                text = "Script Libraries に追加した内容はスクリプトの先頭に追加されます。",
                messageType = HelpBoxMessageType.Info
            });
            container.Add(new PropertyField(scriptContexts)
            {
                name = "PropertyField:" + scriptContexts.propertyPath
            });
            container.Add(new HelpBox
            {
                text = "Script Context に追加した内容が共存します。",
                messageType = HelpBoxMessageType.Info
            });
            var advanced = new Foldout
            {
                text = "上級者向け設定",
                value = _advanced
            };
            advanced.Add(new PropertyField(detectCallbackSupport)
            {
                name = "PropertyField:" + detectCallbackSupport.propertyPath
            });
            advanced.Add(new HelpBox
            {
                text = "オンの場合、コールバックのサポートを必要に応じて生成します。",
                messageType = HelpBoxMessageType.Info
            });
            advanced.RegisterValueChangedCallback(evt => _advanced = evt.newValue);
            container.Add(advanced);
            return container;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scriptLibraries = property.FindPropertyRelative(nameof(JavaScriptSource.scriptLibraries));
            var scriptLibrariesPos = position;
            scriptLibrariesPos.height = EditorGUI.GetPropertyHeight(scriptLibraries);
            EditorGUI.PropertyField(scriptLibrariesPos, scriptLibraries);

            var scriptContexts = property.FindPropertyRelative(nameof(JavaScriptSource.scriptContexts));
            var scriptContextsPos = position;
            scriptContextsPos.yMin = scriptContextsPos.yMax - EditorGUI.GetPropertyHeight(scriptContexts);
            EditorGUI.PropertyField(scriptContextsPos, scriptContexts);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(JavaScriptSource.scriptLibraries))) +
                EditorGUIUtility.standardVerticalSpacing +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(JavaScriptSource.scriptContexts)));
        }
    }
}
