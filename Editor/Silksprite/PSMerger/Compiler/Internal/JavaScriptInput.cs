using System.IO;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.SourcemapAccess;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptInput
    {
        public readonly string SourceCode;
        readonly string _sourceCodePath;

        public SourcemapAsset Sourcemap => 
            _sourceCodePath switch
            {
                not null => SourcemapAsset.CreateIdentity(Path.GetFileName(_sourceCodePath), _sourceCodePath, SourceCode),
                _ => SourcemapAsset.CreateInline(SourceCode)
            };
        
        JavaScriptInput(JavaScriptAsset asset)
        {
            SourceCode = asset.text;
            _sourceCodePath = AssetDatabase.GetAssetPath(asset);
        }

        JavaScriptInput(string sourceCode)
        {
            SourceCode = sourceCode;
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
