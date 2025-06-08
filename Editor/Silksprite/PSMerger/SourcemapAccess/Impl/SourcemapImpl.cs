using System.Linq;
using Silksprite.PSMerger.SourcemapAccess.Base;
using SourcemapToolkit.SourcemapParser;

namespace Silksprite.PSMerger.SourcemapAccess.Impl
{
    public class SourcemapImpl : ISourcemap
    {
        readonly SourceMap _sourceMap;

        public SourcemapImpl(string sourceFileName)
        {
            _sourceMap = new()
            {
                Version = 0,
                File = sourceFileName + ".map",
                Sources = new(),
                Names = new(),
                ParsedMappings = new()
            };
        }
        
        public SourcemapImpl(string sourceFileName, string sourceCode)
        {
            _sourceMap = new SourceMap
            {
                Version = 3,
                File = sourceFileName + ".map",
                Sources = new() { sourceFileName },
                Names = new(),
                ParsedMappings = sourceCode.Split("\n").Select((_, index) => new MappingEntry
                {
                    GeneratedSourcePosition = new SourcePosition
                    {
                        ZeroBasedLineNumber = index,
                        ZeroBasedColumnNumber = 0
                    },
                    OriginalSourcePosition = null,
                    OriginalName = null,
                    OriginalFileName = null
                }).ToList()
            };
        }

        void ISourcemap.AppendLine()
        {
            _sourceMap.ParsedMappings.Add(new MappingEntry
            {
                GeneratedSourcePosition = new SourcePosition
                {
                    ZeroBasedLineNumber = _sourceMap.ParsedMappings.Count,
                    ZeroBasedColumnNumber = 0
                },
                OriginalSourcePosition = null,
                OriginalName = null,
                OriginalFileName = null
            });
        }

        string ISourcemap.Serialize()
        {
            return new SourceMapGenerator().SerializeMapping(_sourceMap);
        }
    }
}
