using System.IO;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.SourcemapAccess;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptInput
    {
        public readonly string SourceCode;
        public readonly SourcemapAsset Sourcemap;

        JavaScriptInput(JavaScriptAsset asset)
        {
            SourceCode = asset.text;
            var assetPath = AssetDatabase.GetAssetPath(asset); 
            Sourcemap = SourcemapAsset.CreateIdentity(Path.GetFileName(assetPath), assetPath, asset.text);
        }

        JavaScriptInput(string sourceCode)
        {
            SourceCode = sourceCode;
            Sourcemap = SourcemapAsset.CreateInline(sourceCode);
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
