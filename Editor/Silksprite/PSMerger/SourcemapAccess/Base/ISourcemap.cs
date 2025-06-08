namespace Silksprite.PSMerger.SourcemapAccess.Base
{
    public interface ISourcemap
    {
        void AppendLine();

        string Serialize();
    }
}
