using System;
using System.Diagnostics.CodeAnalysis;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSMerger.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class ScriptableItemAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
            
        public bool hasModifiedProperties => _serializedObject.hasModifiedProperties;

        public JavaScriptAsset sourceCodeAsset
        {
            set
            {
                using var prop = _serializedObject.FindProperty("sourceCodeAsset");
                if (prop.objectReferenceValue != value) prop.objectReferenceValue = value;
            }
        }

        public string sourceCode
        {
            set
            {
                using var prop = _serializedObject.FindProperty("sourceCode");
                if (prop.stringValue != value) prop.stringValue = value;
            }
        }

        public ScriptableItemAccess(ScriptableItem scriptableItem) => _serializedObject = new SerializedObject(scriptableItem);

        void IDisposable.Dispose() => _serializedObject.ApplyModifiedProperties();
    }
}