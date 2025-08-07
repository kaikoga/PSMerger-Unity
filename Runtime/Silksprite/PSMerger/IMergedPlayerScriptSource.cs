using System.Collections.Generic;

namespace Silksprite.PSMerger
{
    public interface IMergedPlayerScriptSource
    {
        public IEnumerable<JavaScriptSource> JavaScriptSources();
    }
}
