namespace Silksprite.PSMerger.SourcemapAccess.Base
{
    public interface ISourcemapFactory
    {
        ISourcemap Create(string sourceFileName);
        ISourcemap Create(string sourceFileName, string sourceCode);
    }

}
