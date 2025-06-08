using System.IO;
using System.Text;
using Silksprite.PSMerger.SourcemapAccess;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerOutput
    {
        readonly StringBuilder _sourceCode = new();

        ISourcemap _sourcemap = SourcemapFactory.Create();
        
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
            AppendLines(input.Text);
        }

        public string SourceCode() => _sourceCode.ToString();
        public string Sourcemap() => _sourcemap.Serialize();
    }
}
