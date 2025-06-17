using System.IO;
using System.Text;
using Silksprite.PSMerger.SourcemapAccess;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerOutput
    {
        readonly StringBuilder _sourceCode = new();
        readonly ISourcemap _sourcemap;

        public JavaScriptCompilerOutput(string fileName, string assetPath)
        {
            _sourcemap = SourcemapFactory.CreateEmpty(fileName, assetPath);
        }
        
        public void AppendLine(string line)
        {
            _sourceCode.AppendLine(line);
            _sourcemap.AppendLine();
        }

        public void AppendLines(string lines)
        {
            var stringReader = new StringReader(lines);
            while (stringReader.ReadLine() is { } line)
            {
                AppendLine(line);
            }
        }

        public void AppendInput(JavaScriptInput input)
        {
            var stringReader = new StringReader(input.Text);
            while (stringReader.ReadLine() is { } line)
            {
                _sourceCode.AppendLine(line);
            }
            _sourcemap.Concat(input.Sourcemap);
        }

        public string SourceCode() => _sourceCode.ToString();
        public string Sourcemap() => _sourcemap.Serialize();
    }
}
