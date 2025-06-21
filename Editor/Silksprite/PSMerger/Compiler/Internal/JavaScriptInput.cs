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

        public JavaScriptInput(JavaScriptAsset asset)
        {
            SourceCode = asset.text;
            var assetPath = AssetDatabase.GetAssetPath(asset); 
            Sourcemap = SourcemapAsset.CreateIdentity(Path.GetFileName(assetPath), assetPath, asset.text);
        }

        public JavaScriptInput(string fileName, string sourceCode)
        {
            SourceCode = sourceCode;
            Sourcemap = SourcemapAsset.CreateIdentity(fileName, "", sourceCode);
        }
    }
}
