using System.IO;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.SourcemapAccess;
using Silksprite.PSMerger.SourcemapAccess.Base;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptInput
    {
        public readonly JavaScriptAsset JavaScriptAsset;
        public readonly ISourcemap Sourcemap;
 
        public string Text => JavaScriptAsset.text;

        public JavaScriptInput(JavaScriptAsset asset)
        {
            JavaScriptAsset = asset;
            var assetPath = AssetDatabase.GetAssetPath(asset); 
            Sourcemap = SourcemapFactory.CreateIdentity(Path.GetFileName(assetPath), assetPath, asset.text);
        }
    }
}
