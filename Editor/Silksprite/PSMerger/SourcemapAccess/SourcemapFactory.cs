using Silksprite.PSMerger.SourcemapAccess.Base;

#if PSMERGER_SOURCEMAP_SUPPORT
using TSourcemapFactory = Silksprite.PSMerger.SourcemapAccess.Impl.SourcemapFactoryImpl;
#else
using TSourcemapFactory = Silksprite.PSMerger.SourcemapAccess.EmptySourcemapFactory;
#endif

namespace Silksprite.PSMerger.SourcemapAccess
{
    public static class SourcemapFactory
    {
        static readonly ISourcemapFactory Factory = new TSourcemapFactory();        

        public static ISourcemap Create()
        {
            return Factory.Create();
        }
        
        public static ISourcemap Create(string fileName, string sourceCode)
        {
            return Factory.Create(fileName, sourceCode);
        }
    }
}
