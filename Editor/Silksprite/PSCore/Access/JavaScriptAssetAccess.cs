using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class JavaScriptAssetAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
        string _sourcemap;

        public bool hasModifiedProperties => _serializedObject.hasModifiedProperties || _sourcemap != null;

        public string text
        {
            set
            {
                using var prop = _serializedObject.FindProperty(nameof(JavaScriptAsset.text));
                if (prop.stringValue != value) prop.stringValue = value;
            }
        }

        public string sourcemap
        {
            set => _sourcemap = value;
        }

        public JavaScriptAssetAccess(JavaScriptAsset javaScriptAsset) => _serializedObject = new SerializedObject(javaScriptAsset);

        void IDisposable.Dispose()
        {
            // if (!_serializedObject.hasModifiedProperties) return;
            var assetPath = AssetDatabase.GetAssetPath(_serializedObject.targetObject);
            if (string.IsNullOrEmpty(assetPath)) return;

            using var prop = _serializedObject.FindProperty(nameof(JavaScriptAsset.text));
            File.WriteAllBytes(assetPath, Encoding.UTF8.GetBytes(prop.stringValue));
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);

            if (_sourcemap != null)
            {
                File.WriteAllBytes($"{assetPath}.map", Encoding.UTF8.GetBytes(_sourcemap));
            }
        }
    }
}