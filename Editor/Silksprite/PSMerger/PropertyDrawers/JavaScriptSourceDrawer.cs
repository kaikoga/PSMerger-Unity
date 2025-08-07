using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSMerger.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(JavaScriptSource))]
    public class JavaScriptSourceDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var scriptLibraries = property.FindPropertyRelative(JavaScriptSource.NameofScriptLibraries);
            var scriptContexts = property.FindPropertyRelative(JavaScriptSource.NameofScriptContexts);

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
            return container;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scriptLibraries = property.FindPropertyRelative(JavaScriptSource.NameofScriptLibraries);
            var scriptLibrariesPos = position;
            scriptLibrariesPos.height = EditorGUI.GetPropertyHeight(scriptLibraries);
            EditorGUI.PropertyField(scriptLibrariesPos, scriptLibraries);

            var scriptContexts = property.FindPropertyRelative(JavaScriptSource.NameofScriptContexts);
            var scriptContextsPos = position;
            scriptContextsPos.yMin = scriptContextsPos.yMax - EditorGUI.GetPropertyHeight(scriptContexts);
            EditorGUI.PropertyField(scriptContextsPos, scriptContexts);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative(JavaScriptSource.NameofScriptLibraries)) +
                EditorGUIUtility.standardVerticalSpacing +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative(JavaScriptSource.NameofScriptContexts));
        }
    }
}
