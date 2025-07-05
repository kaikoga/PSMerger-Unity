using UnityEngine;

namespace Silksprite.PSCollector
{
    public static class MergedIdStringBuilder
    {
        public static string Build(string mergedIdString, GameObject gameObject)
        {
            return mergedIdString.Replace("{name}", gameObject.name);
        }
    }
}
