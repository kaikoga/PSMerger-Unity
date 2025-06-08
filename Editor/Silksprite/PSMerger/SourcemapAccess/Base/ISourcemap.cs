namespace Silksprite.PSMerger.SourcemapAccess.Base
{
    public interface ISourcemap
    {
        void AppendLine();
        void Concat(ISourcemap sourcemap);

        string Serialize();
    }
}
