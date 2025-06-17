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

        public static ISourcemap CreateEmpty(string sourceFileName, string sourceFileAssetPath)
        {
            return Factory.CreateEmpty(sourceFileName, sourceFileAssetPath);
        }
        
        public static ISourcemap CreateIdentity(string sourceFileName, string sourceFileAssetPath, string sourceCode)
        {
            return Factory.CreateIdentity(sourceFileName, sourceFileAssetPath, sourceCode);
        }
    }
}
