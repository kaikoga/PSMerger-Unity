using System.IO;
using System.Linq;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerOutput
    {
        readonly StringBuilder _sb = new();
        
        public void AppendLine(string line)
        {
            _sb.AppendLine(line);
        }

        public void AppendLines(string lines)
        {
            var stringReader = new StringReader(lines);
            while (stringReader.ReadLine() is { } line)
            {
                _sb.AppendLine(line);
            }
        }

        public string SourceCode() => _sb.ToString();
    }
}
