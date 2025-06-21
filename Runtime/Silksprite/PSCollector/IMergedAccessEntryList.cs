using System.Collections.Generic;

namespace Silksprite.PSCollector
{
    public interface IMergedAccessEntryList<out T>
    {
        public IEnumerable<T> Entries { get; }
    }
}
