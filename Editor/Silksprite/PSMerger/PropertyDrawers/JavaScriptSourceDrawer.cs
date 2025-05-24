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
            var scriptLibraries = property.FindPropertyRelative(nameof(JavaScriptSource.scriptLibraries));
            var scriptContexts = property.FindPropertyRelative(nameof(JavaScriptSource.scriptContexts));

            var container = new VisualElement();
            container.Add(new PropertyField(property.FindPropertyRelative(nameof(JavaScriptSource.scriptLibraries)))
            {
                name = "PropertyField:" + scriptLibraries.propertyPath
            });
            container.Add(new PropertyField(property.FindPropertyRelative(nameof(JavaScriptSource.scriptContexts)))
            {
                name = "PropertyField:" + scriptContexts.propertyPath
            });
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
