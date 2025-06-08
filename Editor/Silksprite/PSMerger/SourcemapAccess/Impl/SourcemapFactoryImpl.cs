using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess.Impl
{
    [UsedImplicitly]
    public class SourcemapFactoryImpl : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.Create() => new SourcemapImpl();

        ISourcemap ISourcemapFactory.Create(string fileName, string sourceCode) => new SourcemapImpl(fileName, sourceCode);
    }
}
