using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSMerger.Compiler
{
    public class JavaScriptGenerator
    {
        const string ItemScriptPreamble = @"
const $$ = (() => {
    const h = new Proxy(new Map(), {
        get(target, prop, receiver) {
            return target.has(prop) ? target.get(prop) : target.set(prop, new Map()).get(prop);
        }
    });
    $.onUpdate((...args) => {
        for (const f of h.onUpdate.values()) f(...args);
    });
    $.onReceive((...args) => {
        for (const f of h.onReceive.values()) f(...args);
    }, { item: true, player: true });
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
            onUpdate(callback) { h.onUpdate.set(this, callback) },
            onReceive(callback) { h.onReceive.set(this, callback) },
        }, $);
    }
})();
";

        const string PlayerScriptPreamble = @"
const __ = (() => {
    const h = new Proxy(new Map(), {
        get: function(target, prop) { return target.has(prop) ? target.get(prop) : target.set(prop, new Map()).get(prop); }
    });
    _.onFrame((...args) => {
        for (const f of h.onFrame.values()) f(...args);
    });
    _.onReceive((...args) => {
        for (const f of h.onReceive.values()) f(...args);
    });
    _.oscHandle.onReceive((...args) => {
        for (const f of h.onOscReceive.values()) f(...args);
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
        const _oscHandle = createProxy({
            onOscReceive(callback) { h.onOscReceive.set(this, callback) },
        }, _.oscHandle);
        return createProxy({
            onFrame(callback) { h.onFrame.set(this, callback) },
            onReceive(callback) { h.onReceive.set(this, callback) },
            get oscHandle() { return _oscHandle },
        }, _);
    }
})();
";

        readonly string _preamble;
        readonly string _globalHandle;
        readonly string _globalContextProvider;

        public JavaScriptGenerator(bool playerScript)
        {
            _preamble = playerScript ? PlayerScriptPreamble : ItemScriptPreamble;
            _globalHandle = playerScript ? "_" : "$";
            _globalContextProvider = playerScript ? "__" : "$$";
        }

        public string MergeScripts(JavaScriptAsset[][] scripts)
        {
            return _preamble + string.Join("\n", scripts.Select(context => $@"
({_globalHandle} => {{
{string.Join("\n", context.Select(ps => ps != null ? ps.text : null))}
}})({_globalContextProvider}());
"));
        }
    }
}
