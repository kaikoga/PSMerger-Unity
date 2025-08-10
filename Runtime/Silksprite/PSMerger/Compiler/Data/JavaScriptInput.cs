using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Data
{
    public class JavaScriptInput
    {
        public readonly string SourceCode;
        public readonly string SourceCodePath;

        JavaScriptInput(JavaScriptAsset asset)
        {
            SourceCode = asset.text;
            SourceCodePath = AssetDatabase.GetAssetPath(asset);
        }

        JavaScriptInput(string sourceCode)
        {
            SourceCode = sourceCode;
        }

        public static JavaScriptInput Empty()
        {
            return new JavaScriptInput("");
        }

        public static JavaScriptInput FromAsset(JavaScriptAsset asset)
        {
            return new JavaScriptInput(asset);
        }

        public static JavaScriptInput Inline(string sourceCode)
        {
            return new JavaScriptInput(sourceCode);
        }
    }

}
