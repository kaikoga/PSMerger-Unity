using Silksprite.PSMerger.CompilerAPI.Data;

namespace Silksprite.PSMerger.CompilerAPI.Filter
{
    public interface IPSMergerFilter
    {
        public int Priority { get; }

        JavaScriptCompilerEnvironment Filter(JavaScriptCompilerEnvironment environment, UnityEngine.Object unityContext);
        string PostProcess(string sourceCode, UnityEngine.Object unityContext);
    }
}
