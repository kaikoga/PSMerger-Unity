using System.Collections.Generic;

namespace Silksprite.PSMerger
{
    public interface IMergedClusterScriptSourceBase
    {
        public IEnumerable<JavaScriptSource> JavaScriptSources();
    }
}
