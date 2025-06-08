using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess
{
    [UsedImplicitly]
    public class EmptySourcemapFactory : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.Create(string sourceFileName) => new EmptySourcemap();

        ISourcemap ISourcemapFactory.Create(string sourceFileName, string sourceCode) => new EmptySourcemap();
    }
}
