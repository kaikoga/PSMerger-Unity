using System.IO;
using System.Linq;
using Editor.Silksprite.PSCore.Extensions;
using SourcemapToolkit.SourcemapParser;

namespace Silksprite.PSMerger.SourcemapAccess
{
    public class SourcemapAsset
    {
        readonly SourceMap _sourceMap;
        readonly string _sourceFileAssetPath;

        SourcemapAsset(SourceMap sourceMap, string sourceFileAssetPath)
        {
            _sourceMap = sourceMap;
            _sourceFileAssetPath = sourceFileAssetPath;
        }

        public static SourcemapAsset CreateEmpty(string sourceFileName, string sourceFileAssetPath)
        {
            return new SourcemapAsset(
                new SourceMap
                {
                    Version = 3,
                    File = sourceFileName,
                    Sources = new(),
                    Names = new(),
                    ParsedMappings = new()
                },
                sourceFileAssetPath
            );
        }

        public static SourcemapAsset CreateIdentity(string sourceFileName, string sourceFileAssetPath, string sourceCode)
        {
            return new SourcemapAsset(
                new SourceMap
                {
                    Version = 3,
                    File = sourceFileName,
                    Sources = new() { sourceFileName },
                    Names = new(),
                    ParsedMappings = sourceCode.Lines().Select((_, index) => new MappingEntry
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
                },
                sourceFileAssetPath
            );
        }
        
        public static SourcemapAsset CreateInline(string sourceCode)
        {
            return new SourcemapAsset(
                new SourceMap
                {
                    Version = 3,
                    File = null,
                    Sources = new(),
                    Names = new(),
                    ParsedMappings = sourceCode.Lines().Select((_, index) => new MappingEntry
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
                },
                null
            );
        }

        public void AppendLine()
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

        public void Concat(SourcemapAsset other)
        {
            string ConvertRelativePath(string sourcePath)
            {
                if (string.IsNullOrEmpty(sourcePath))
                {
                    return null;
                }
                if (Path.IsPathRooted(sourcePath))
                {
                    return sourcePath;
                }
                var path = sourcePath;
                if (!string.IsNullOrWhiteSpace(other._sourceFileAssetPath))
                {
                    var inDirectoryName = Path.GetDirectoryName(other._sourceFileAssetPath);
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

            var inSourcemap = other._sourceMap;
            if (inSourcemap.File is { } inFile)
            {
                var file = ConvertRelativePath(inFile);
                if (!_sourceMap.Sources.Contains(file))
                {
                    _sourceMap.Sources.Add(file);
                }
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

        public string Serialize()
        {
            return new SourceMapGenerator().SerializeMapping(_sourceMap);
        }
    }
}
