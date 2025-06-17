namespace Silksprite.PSMerger.SourcemapAccess.Base
{
    public interface ISourcemapFactory
    {
        ISourcemap CreateEmpty(string sourceFileName, string sourceFileAssetPath);
        ISourcemap CreateIdentity(string sourceFileName, string sourceFileAssetPath, string sourceCode);
    }

}
