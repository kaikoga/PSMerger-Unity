using System.IO;
using System.Linq;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class JavaScriptCompilerEnvironment
    {
        public readonly JavaScriptInput[] ScriptLibraries;
        public readonly JavaScriptCompilerContext[] ScriptContexts;
        public readonly bool DetectCallbackSupport;

        readonly StringBuilder _sb = new();
        
        public JavaScriptCompilerEnvironment(JavaScriptSource source)
        {
            ScriptLibraries = source.scriptLibraries
                .Where(asset => asset is not null)
                .Select(asset => new JavaScriptInput(asset))
                .ToArray();
            ScriptContexts = source.scriptContexts
                .Select(context => new JavaScriptCompilerContext(context))
                .ToArray();
            DetectCallbackSupport = source.detectCallbackSupport;
        }
        
        public string[] AllScripts => 
            ScriptLibraries
                .Concat(ScriptContexts.SelectMany(context => context.JavaScriptInputs))
                .Select(input => input.Text)
                .ToArray();
        
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

        public string Output() => _sb.ToString();
    }

    public class JavaScriptCompilerContext
    {
        public readonly JavaScriptInput[] JavaScriptInputs;

        public JavaScriptCompilerContext(JavaScriptContext context)
        {
            JavaScriptInputs = context.JavaScriptAssets
                .Where(asset => asset is not null)
                .Select(asset => new JavaScriptInput(asset))
                .ToArray();
        }
    }

    public class JavaScriptInput
    {
        public readonly JavaScriptAsset JavaScriptAsset;

        public string Text => JavaScriptAsset.text;

        public JavaScriptInput(JavaScriptAsset asset)
        {
            JavaScriptAsset = asset;
        }
    }
}
