using System;

namespace Silksprite.PSMerger.Compiler
{
    public static class ClusterScriptAssetMergerCompiler
    {
        public static void Compile(ClusterScriptAssetMerger merger)
        {
            if (merger.MergedScript == null)
            {
                return;                
            }
            switch (merger.ScriptType)
            {
                case ClusterScriptType.ConcatOnly:
                    ConcatOnlyCompiler.Compile(merger);
                    break;
                case ClusterScriptType.ItemScript:
                    ItemScriptMergerCompiler.Compile(merger);
                    break;
                case ClusterScriptType.PlayerScript:
                    PlayerScriptMergerCompiler.Compile(merger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(merger.ScriptType), merger.ScriptType, null);
            }
        }
    }
}