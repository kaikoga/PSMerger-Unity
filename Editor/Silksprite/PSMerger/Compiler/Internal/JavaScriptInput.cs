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
        public ISourcemap Sourcemap;
 
        public string Text => JavaScriptAsset.text;

        public JavaScriptInput(JavaScriptAsset asset)
        {
            JavaScriptAsset = asset;
            Sourcemap = SourcemapFactory.Create(Path.GetFileName(AssetDatabase.GetAssetPath(asset)), asset.text);
        }
    }
}
