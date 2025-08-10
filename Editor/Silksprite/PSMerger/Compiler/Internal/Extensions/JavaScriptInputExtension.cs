using System.IO;
using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.SourcemapAccess;

namespace Silksprite.PSMerger.Compiler.Internal.Extensions
{
    public static class JavaScriptInputExtension
    {
        public static SourcemapAsset Sourcemap(this JavaScriptInput javaScriptInput) =>
            javaScriptInput.SourceCodePath switch
            {
                not null => SourcemapAsset.CreateIdentity(Path.GetFileName(javaScriptInput.SourceCodePath), javaScriptInput.SourceCodePath, javaScriptInput.SourceCode),
                _ => SourcemapAsset.CreateInline(javaScriptInput.SourceCode)
            };

    }
}
