using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSMerger.Compiler
{
    public class JavaScriptGenerator
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
            onUpdate(callback) { onUpdate.set(this, callback) },
            onReceive(callback) { onReceive.set(this, callback) },
        }, $);
    }
})();
";

        const string PlayerScriptPreamble = @"
const __ = (() => {
    const onFrame = new Map();
    const onReceive = new Map();
    const onOscReceive = new Map();
    _.onFrame(deltaTime => {
        for (const f of onFrame.values()) f(deltaTime);
    });
    _.onReceive((messageType, arg, sender) => {
        for (const f of onReceive.values()) f(messageType, arg, sender);
    });
    _.oscHandle.onReceive((messages) => {
        for (const f of onOscReceive.values()) f(messages);
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
            onOscReceive(callback) { onOscReceive.set(this, callback) },
        }, _.oscHandle);
        return createProxy({
            onFrame(callback) { onFrame.set(this, callback) },
            onReceive(callback) { onReceive.set(this, callback) },
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
