using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Compiler.Data;

namespace Silksprite.PSMerger.Compiler.Extensions
{
    static class JavaScriptAssetExtension
    {
        public static IEnumerable<JavaScriptInput> ToJavaScriptInputs(this IEnumerable<JavaScriptAsset> assets, string defaultSourceCode)
        {
            return assets.Select(asset => asset
                    ? JavaScriptInput.FromAsset(asset)
                    : !string.IsNullOrEmpty(defaultSourceCode)
                        ? JavaScriptInput.Inline(defaultSourceCode)
                        : null)
                .Where(input => input != null);
        }
    }
}
