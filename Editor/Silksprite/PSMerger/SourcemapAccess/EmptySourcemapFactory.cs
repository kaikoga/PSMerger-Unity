using JetBrains.Annotations;
using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess
{
    [UsedImplicitly]
    public class EmptySourcemapFactory : ISourcemapFactory
    {
        ISourcemap ISourcemapFactory.CreateEmpty(string sourceFileName, string sourceFileAssetPath) => new EmptySourcemap();

        ISourcemap ISourcemapFactory.CreateIdentity(string sourceFileName, string sourceFileAssetPath, string sourceCode) => new EmptySourcemap();
    }
}
