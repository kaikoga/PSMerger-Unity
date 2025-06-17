using System;
using System.IO;
using System.Linq;
using Silksprite.PSMerger.SourcemapAccess.Base;
using SourcemapToolkit.SourcemapParser;

namespace Silksprite.PSMerger.SourcemapAccess.Impl
{
    public class SourcemapImpl : ISourcemap
    {
        readonly SourceMap _sourceMap;
        readonly string _sourceFileAssetPath;

        public SourcemapImpl(string sourceFileName, string sourceFileAssetPath)
        {
            _sourceMap = new()
            {
                Version = 3,
                File = sourceFileName,
                Sources = new(),
                Names = new(),
                ParsedMappings = new()
            };
            _sourceFileAssetPath = sourceFileAssetPath;
        }
        
        public SourcemapImpl(string sourceFileName, string sourceFileAssetPath, string sourceCode)
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
            _sourceFileAssetPath = sourceFileAssetPath;
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

            string ConvertRelativePath(string sourcePath)
            {
                if (Path.IsPathRooted(sourcePath))
                {
                    return sourcePath;
                }
                var path = sourcePath;
                if (!string.IsNullOrWhiteSpace(impl._sourceFileAssetPath))
                {
                    var inDirectoryName = Path.GetDirectoryName(impl._sourceFileAssetPath);
                    if (inDirectoryName != null)
                    {
                        if (Path.IsPathRooted(inDirectoryName))
                        {
                            return Path.Combine(inDirectoryName, path);
                        }
                        path = Path.GetFullPath(Path.Combine(inDirectoryName, path));
                    }
                    
                }
                if (!string.IsNullOrWhiteSpace(_sourceFileAssetPath))
                {
                    var outDirectoryName = Path.GetDirectoryName(_sourceFileAssetPath);
                    if (outDirectoryName != null)
                    {
                        path = Path.GetRelativePath(outDirectoryName, path);
                    }
                }
                return path;
            }

            var lineStartIndex = _sourceMap.ParsedMappings.Any()
                ? _sourceMap.ParsedMappings.Max(mapping => mapping.GeneratedSourcePosition.ZeroBasedLineNumber) + 1
                : 0;

            var inSourcemap = impl._sourceMap;
            var inFile = ConvertRelativePath(inSourcemap.File);
            if (!_sourceMap.Sources.Contains(inFile))
            {
                _sourceMap.Sources.Add(inFile);
            }
            _sourceMap.Sources = _sourceMap.Sources
                .Concat(inSourcemap.Sources
                    .Select(ConvertRelativePath))
                .Distinct().ToList();
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
                    OriginalFileName = ConvertRelativePath(mapping.OriginalFileName ?? inSourcemap.File)
                }));
        }

        string ISourcemap.Serialize()
        {
            return new SourceMapGenerator().SerializeMapping(_sourceMap);
        }
    }
}
