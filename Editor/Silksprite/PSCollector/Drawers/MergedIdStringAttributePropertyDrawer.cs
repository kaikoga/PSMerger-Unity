using System.Linq;
using Silksprite.PSCollector;
using Silksprite.PSCollector.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Silksprite.PSCore.Drawers
{
    [CustomPropertyDrawer(typeof(MergedIdStringAttribute), true)]
    public class MergedIdStringAttributePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return CreateIdStringPropertyGUI(property, property.displayName);
        }

        static VisualElement CreateIdStringPropertyGUI(SerializedProperty property, string displayName = "")
        {
            Assert.AreEqual(property.propertyType, SerializedPropertyType.String);
            var container = new VisualElement();

            var validationErrorsContainer = new VisualElement();
            var validator = new MergedIdStringValidator(property.displayName);

            var idField = new TextField("Merged Id");
            idField.SetEnabled(false);

            void RenderValidationErrors()
            {
                idField.style.display = DisplayStyle.None;
                validationErrorsContainer.Clear();
                if (property.hasMultipleDifferentValues)
                {
                    return;
                }
                var targetGameObjects = property.serializedObject.targetObjects
                    .OfType<Component>()
                    .Select(c => c.gameObject)
                    .Distinct()
                    .ToArray();
                if (targetGameObjects.Length != 1)
                {
                    return;
                }
                var mergedIdString = property.stringValue;
                var idString = MergedIdStringBuilder.Build(mergedIdString, targetGameObjects[0]);
                if (!validator.IsValid(idString, out var validationErrors))
                {
                    foreach (var error in validationErrors)
                    {
                        validationErrorsContainer.Add(new HelpBox(error, HelpBoxMessageType.Error));
                    }
                }
                if (idString != mergedIdString)
                {
                    idField.style.display = DisplayStyle.Flex;
                    idField.value = idString;
                }
            }

            RenderValidationErrors();
            var mergedIdField = new TextField(displayName)
            {
                bindingPath = property.propertyPath
            };
            mergedIdField.Bind(property.serializedObject);
            mergedIdField.RegisterCallback<ChangeEvent<string>>(_ => RenderValidationErrors());

            container.Add(validationErrorsContainer);
            container.Add(mergedIdField);
            container.Add(idField);
            return container;
        }
    }
}
