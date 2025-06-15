using System;
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
                Version = 3,
                File = sourceFileName,
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
                File = sourceFileName,
                Sources = new() { sourceFileName },
                Names = new(),
                ParsedMappings = sourceCode.Split("\n").Select((_, index) => new MappingEntry
                {
                    GeneratedSourcePosition = new SourcePosition
                    {
                        ZeroBasedLineNumber = index,
                        ZeroBasedColumnNumber = 0
                    },
                    OriginalSourcePosition = new SourcePosition
                    {
                        ZeroBasedLineNumber = index,
                        ZeroBasedColumnNumber = 0
                    },
                    OriginalName = null,
                    OriginalFileName = sourceFileName
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

        void ISourcemap.Concat(ISourcemap sourcemap)
        {
            if (sourcemap is not SourcemapImpl impl)
            {
                throw new NotSupportedException();
            }

            var lineStartIndex = _sourceMap.ParsedMappings.Any()
                ? _sourceMap.ParsedMappings.Max(mapping => mapping.GeneratedSourcePosition.ZeroBasedLineNumber) + 1
                : 0;

            var inSourcemap = impl._sourceMap;
            if (!_sourceMap.Sources.Contains(inSourcemap.File))
            {
                _sourceMap.Sources.Add(inSourcemap.File);
            }
            _sourceMap.Sources = _sourceMap.Sources.Concat(inSourcemap.Sources).Distinct().ToList();
            _sourceMap.Names = _sourceMap.Names.Concat(inSourcemap.Names).Distinct().ToList();
            _sourceMap.ParsedMappings.AddRange(inSourcemap.ParsedMappings
                .Select(mapping => new MappingEntry
                {
                    GeneratedSourcePosition = new ()
                    {
                        ZeroBasedLineNumber = mapping.GeneratedSourcePosition.ZeroBasedLineNumber + lineStartIndex,
                        ZeroBasedColumnNumber = mapping.GeneratedSourcePosition.ZeroBasedColumnNumber
                    },
                    OriginalSourcePosition = (mapping.OriginalFileName != null ? mapping.OriginalSourcePosition : mapping.GeneratedSourcePosition).Clone(),
                    OriginalName = mapping.OriginalName,
                    OriginalFileName = mapping.OriginalFileName ?? inSourcemap.File
                }));
        }

        string ISourcemap.Serialize()
        {
            return new SourceMapGenerator().SerializeMapping(_sourceMap);
        }
    }
}
