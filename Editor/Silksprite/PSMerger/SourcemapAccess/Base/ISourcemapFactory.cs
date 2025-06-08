namespace Silksprite.PSMerger.SourcemapAccess.Base
{
    public interface ISourcemapFactory
    {
        ISourcemap Create();
        ISourcemap Create(string fileName, string sourceCode);
    }

}
