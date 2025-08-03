using System.Collections.Generic;
using System.IO;

namespace Editor.Silksprite.PSCore.Extensions
{
    public static class StringExtension
    {
        public static IEnumerable<string> Lines(this string lines)
        {
            var stringReader = new StringReader(lines);
            while (stringReader.ReadLine() is { } line)
            {
                yield return line;
            }
        }
    }
}
