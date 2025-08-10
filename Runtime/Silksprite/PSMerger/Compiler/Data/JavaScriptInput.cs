using System.IO;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Data
{
    public class JavaScriptInput
    {
        public readonly string SourceCode;
        public readonly string SourceCodePath;
        public readonly string Sourcemap;

        public JavaScriptInput(string sourceCode, string sourceCodePath = null, string sourcemap = null)
        {
            SourceCode = sourceCode;
            SourceCodePath = sourceCodePath;
            Sourcemap = sourcemap;
        }

        public static JavaScriptInput Empty()
        {
            return new JavaScriptInput("");
        }

        public static JavaScriptInput Inline(string sourceCode)
        {
            return new JavaScriptInput(sourceCode);
        }

        public static JavaScriptInput FromAsset(JavaScriptAsset asset)
        {
            var sourceCode = asset.text;
            var sourceCodePath = AssetDatabase.GetAssetPath(asset);
            var sourcemapPath = $"{sourceCodePath}.map";
            var sourcemap = File.Exists(sourcemapPath) ? File.ReadAllText(sourcemapPath, System.Text.Encoding.UTF8) : null;
            return new JavaScriptInput(sourceCode, sourceCodePath, sourcemap);
        }
    }

}
