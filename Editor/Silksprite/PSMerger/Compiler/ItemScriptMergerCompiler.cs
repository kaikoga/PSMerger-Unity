using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger.Compiler
{
    public static class ItemScriptMergerCompiler
    {
        const string ItemScriptPreamble = @"
const $$ = (() => {
    const onUpdate = new Map();
    const onReceive = new Map();
    $.onUpdate(deltaTime => {
        for (const f of onFrame.values()) f(deltaTime);
    });
    $.onReceive((messageType, arg, sender) => {
        for (const f of onReceive.values()) f(messageType, arg, sender);
    });
    function createProxy(obj, base) {
        return new Proxy(obj, {
            get(target, prop, receiver) {
                let thisValue = target; 
                let value = target[prop];
                if (value === undefined) {
                    thisValue = base;
                    value = base[prop];
                }
                return (value instanceof Function) ? value.bind(thisValue) : value;
            }
        });
    }
    return () => {
        return createProxy({
            onUpdate(callback) { onUpdate.set(this, callback) },
            onReceive(callback) { onReceive.set(this, callback) },
        }, $);
    }
})();
";

        public static bool Compile(ClusterScriptAssetMerger clusterScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(clusterScriptAssetMerger.MergedScript);
            javaScriptAssetAccess.text = BuildItemScript(clusterScriptAssetMerger.ScriptContexts);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildItemScript(JavaScriptAsset[][] itemScripts)
        {
            return ItemScriptPreamble + string.Join("\n", itemScripts.Select(context => $@"
($ => {{
{string.Join("\n", context.Select(ps => ps != null ? ps.text : null))}
}})($$());
"));
        }
    }
}
