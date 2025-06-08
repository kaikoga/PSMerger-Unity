using Silksprite.PSMerger.SourcemapAccess.Base;

namespace Silksprite.PSMerger.SourcemapAccess
{
    public class EmptySourcemap : ISourcemap
    {
        void ISourcemap.AppendLine() { }
        string ISourcemap.Serialize() => "";
    }
}
