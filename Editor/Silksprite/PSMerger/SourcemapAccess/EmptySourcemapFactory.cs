using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess
{
    [UsedImplicitly]
    public class EmptySourcemapFactory : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.Create() => new EmptySourcemap();

        ISourcemap ISourcemapFactory.Create(string fileName, string sourceCode) => new EmptySourcemap();
    }
}
