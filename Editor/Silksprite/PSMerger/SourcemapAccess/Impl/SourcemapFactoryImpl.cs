using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess.Impl
{
    [UsedImplicitly]
    public class SourcemapFactoryImpl : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.Create(string sourceFileName) => new SourcemapImpl(sourceFileName);

        ISourcemap ISourcemapFactory.Create(string sourceFileName, string sourceCode) => new SourcemapImpl(sourceFileName, sourceCode);
    }
}
