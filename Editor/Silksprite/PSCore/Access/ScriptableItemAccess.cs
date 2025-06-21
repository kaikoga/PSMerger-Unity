using System.Diagnostics.CodeAnalysis;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ScriptableItemAccess : ObjectAccessBase<ScriptableItem>
    {
        public JavaScriptAsset sourceCodeAsset
        {
            set
            {
                using var prop = serializedObject.FindProperty("sourceCodeAsset");
                if (prop.objectReferenceValue != value) prop.objectReferenceValue = value;
            }
        }

        public string sourceCode
        {
            set
            {
                using var prop = serializedObject.FindProperty("sourceCode");
                if (prop.stringValue != value) prop.stringValue = value;
            }
        }

        public ScriptableItemAccess(ScriptableItem scriptableItem) : base(scriptableItem)
        {
        }
    }
}