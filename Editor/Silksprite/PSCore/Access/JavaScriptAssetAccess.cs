using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;
using UnityEditor;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class JavaScriptAssetAccess : ObjectAccessBase<JavaScriptAsset>
    {
        string _sourcemap;

        public string text
        {
            set
            {
                using var prop = serializedObject.FindProperty(nameof(JavaScriptAsset.text));
                if (prop.stringValue != value) prop.stringValue = value;
            }
        }

        public string sourcemap
        {
            set => _sourcemap = value;
        }

        public JavaScriptAssetAccess(JavaScriptAsset javaScriptAsset) : base(javaScriptAsset)
        {
        }

        public override void Dispose()
        {
            // if (!_serializedObject.hasModifiedProperties) return;
            var assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            if (string.IsNullOrEmpty(assetPath)) return;

            using var prop = serializedObject.FindProperty(nameof(JavaScriptAsset.text));
            File.WriteAllBytes(assetPath, Encoding.UTF8.GetBytes(prop.stringValue));
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);

            if (_sourcemap != null)
            {
                File.WriteAllBytes($"{assetPath}.map", Encoding.UTF8.GetBytes(_sourcemap));
            }
        }
    }
}