using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess.Impl
{
    [UsedImplicitly]
    public class SourcemapFactoryImpl : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.CreateEmpty(string sourceFileName, string sourceFileAssetPath)
        {
            return new SourcemapImpl(sourceFileName, sourceFileAssetPath);
        }

        ISourcemap ISourcemapFactory.CreateIdentity(string sourceFileName, string sourceFileAssetPath, string sourceCode)
        {
            return new SourcemapImpl(sourceFileName, sourceFileAssetPath, sourceCode);
        }
    }
}
