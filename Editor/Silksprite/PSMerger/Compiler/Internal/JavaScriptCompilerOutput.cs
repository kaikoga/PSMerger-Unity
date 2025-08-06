using System.IO;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;
using Editor.Silksprite.PSCore.Extensions;
using Silksprite.PSMerger.SourcemapAccess;
using UnityEditor;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerOutput
    {
        readonly StringBuilder _sourceCode = new();
        readonly SourcemapAsset _sourcemap;

        public static JavaScriptCompilerOutput CreateFromAssetOutput(JavaScriptAsset assetOutput)
        {
            if (assetOutput)
            {
                var assetPath = AssetDatabase.GetAssetPath(assetOutput);
                return new JavaScriptCompilerOutput(Path.GetFileName(assetPath), assetPath);
            }
            else
            {
                return new JavaScriptCompilerOutput();
            }
        }

        JavaScriptCompilerOutput()
        {
        }

        JavaScriptCompilerOutput(string fileName, string assetPath)
        {
            _sourcemap = SourcemapAsset.CreateEmpty(fileName, assetPath);
        }
        
        public void AppendLine(string line)
        {
            _sourceCode.AppendLine(line);
            _sourcemap?.AppendLine();
        }

        public void AppendLines(string lines)
        {
            foreach (var line in lines.Lines())
            {
                AppendLine(line);
            }
        }

        public void AppendInput(JavaScriptInput input)
        {
            foreach (var line in input.SourceCode.Lines())
            {
                _sourceCode.AppendLine(line);
            }
            _sourcemap?.Concat(input.Sourcemap);
        }

        public string SourceCode() => _sourceCode.ToString();
        public string Sourcemap() => _sourcemap?.Serialize() ?? "";
    }
}
