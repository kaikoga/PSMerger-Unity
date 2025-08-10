using System.Collections.Generic;
using System.Linq;
using Silksprite.PSMerger.Compiler.Data;

namespace Silksprite.PSMerger.Compiler.Filter
{
    public static class PSMergerFilter
    {
        static readonly SortedSet<IPSMergerFilter> Filters = new SortedSet<IPSMergerFilter>(new Comparer());

        public static void Register(IPSMergerFilter filter)
        {
            if (filter != null)
            {
                Filters.Add(filter);
            }
        }

        public static JavaScriptCompilerEnvironment Apply(JavaScriptCompilerEnvironment environment, UnityEngine.Object unityContext)
        {
            return Filters.Aggregate(environment, (env, filter) => filter.Filter(env, unityContext));
        }

        public static string ApplyPostProcess(string sourceCode, UnityEngine.Object unityContext)
        {
            return Filters.Aggregate(sourceCode, (source, filter) => filter.PostProcess(source, unityContext));
        }

        class Comparer : IComparer<IPSMergerFilter>
        {
            public int Compare(IPSMergerFilter x, IPSMergerFilter y)
            {
                if (ReferenceEquals(x, y))
                    return 0;
                if (y is null)
                    return 1;
                if (x is null)
                    return -1;
                return x.Priority.CompareTo(y.Priority);
            }
        }
    }
}
