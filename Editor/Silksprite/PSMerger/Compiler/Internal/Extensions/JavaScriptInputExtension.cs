using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.SourcemapAccess;

namespace Silksprite.PSMerger.Compiler.Internal.Extensions
{
    public static class JavaScriptInputExtension
    {
        public static SourcemapAsset Sourcemap(this JavaScriptInput javaScriptInput) =>
            (javaScriptInput.SourceCodePath, javaScriptInput.Sourcemap) switch
            {
                ({ } sourceCodePath, { } sourcemap)
                    => SourcemapAsset.CreateWithSourcemap(sourceCodePath, sourcemap),
                ({ } sourceCodePath, null)
                    => SourcemapAsset.CreateIdentity(sourceCodePath, javaScriptInput.SourceCode),
                _
                    => SourcemapAsset.CreateInline(javaScriptInput.SourceCode)
            };

    }
}
