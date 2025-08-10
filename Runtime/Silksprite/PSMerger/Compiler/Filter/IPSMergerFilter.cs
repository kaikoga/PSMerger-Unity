using Silksprite.PSMerger.Compiler.Data;

namespace Silksprite.PSMerger.Compiler.Filter
{
    public interface IPSMergerFilter
    {
        public int Priority { get; }

        JavaScriptCompilerEnvironment Filter(JavaScriptCompilerEnvironment environment, UnityEngine.Object unityContext);
        string PostProcess(string sourceCode, UnityEngine.Object unityContext);
    }
}
