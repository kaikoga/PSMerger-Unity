using System;

namespace Silksprite.PSMerger.Compiler
{
    public static class ClusterScriptAssetMergerCompiler
    {
        public static bool Compile(ClusterScriptAssetMerger merger)
        {
            if (merger.MergedScript == null)
            {
                return false;
            }
            return merger.ScriptType switch
            {
                ClusterScriptType.ConcatOnly => ConcatOnlyCompiler.Compile(merger),
                ClusterScriptType.ItemScript => ItemScriptMergerCompiler.Compile(merger),
                ClusterScriptType.PlayerScript => PlayerScriptMergerCompiler.Compile(merger),
                _ => throw new ArgumentOutOfRangeException(nameof(merger.ScriptType), merger.ScriptType, null)
            };
        }
    }
}