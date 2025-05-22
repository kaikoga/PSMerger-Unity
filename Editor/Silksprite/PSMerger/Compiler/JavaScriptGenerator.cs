using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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

        
        readonly string _g;
        readonly string _gg;
        readonly string _preamble;

        public JavaScriptGenerator(bool playerScript)
        {
            _g = playerScript ? "_" : "$";
            _gg = playerScript ? "__" : "$$";
            _preamble = playerScript 
                ? BuildPreamble(new (string, string, string)[]
                {
                    (null, "onFrame", null),
                    (null, "onReceive", null),
                    ("oscHandle", "onOscReceive", null)
                })
                : ItemScriptPreamble;
        }

        public string MergeScripts(JavaScriptAsset[][] scripts)
        {
            return _preamble + string.Join("\n", scripts.Select(context => $@"
({_g} => {{
{string.Join("\n", context.Select(ps => ps != null ? ps.text : null))}
}})({_gg}());
"));
        }

        [SuppressMessage("ReSharper", "RedundantStringInterpolation")]
        string BuildPreamble((string obj, string name, string args)[] callbackDefs)
        {
            var callbacks = callbackDefs
                .Select(def => (def.obj, def.name, path: def.obj is null ? def.name : $"{def.obj}.{def.name}", def.args))
                .ToArray();
            var objs = callbacks
                .Where(c => c.obj is not null)
                .GroupBy(c => c.obj)
                .ToArray();
            var sb = new StringBuilder();
            
            sb.AppendLine($"const {_gg} = (() => {{");
            sb.AppendLine($"  const h = new Proxy(new Map(), {{");
            sb.AppendLine($"    get: function(target, prop) {{ return target.has(prop) ? target.get(prop) : target.set(prop, new Map()).get(prop); }}");
            sb.AppendLine($"  }});");
            foreach (var callback in callbacks)
            {
                sb.AppendLine($"  {_g}.{callback.path}((...args) => {{");
                sb.AppendLine($"    for (const f of h.{callback.name}.values()) f(...args);");
                sb.AppendLine($"  }});");
            }
            sb.AppendLine($"  function createProxy(obj, base) {{");
            sb.AppendLine($"    return new Proxy(obj, {{");
            sb.AppendLine($"      get(target, prop, receiver) {{");
            sb.AppendLine($"        let thisValue = target; ");
            sb.AppendLine($"        let value = target[prop];");
            sb.AppendLine($"        if (value === undefined) {{");
            sb.AppendLine($"          thisValue = base;");
            sb.AppendLine($"          value = base[prop];");
            sb.AppendLine($"        }}");
            sb.AppendLine($"        return (value instanceof Function) ? value.bind(thisValue) : value;");
            sb.AppendLine($"      }}");
            sb.AppendLine($"    }});");
            sb.AppendLine($"  }}");
            sb.AppendLine($"  return () => {{");
            foreach (var obj in objs)
            {
                sb.AppendLine($"    const _{obj.Key} = createProxy({{");
                foreach (var c in obj)
                {
                    sb.AppendLine($"      {c.name}(callback) {{ h.{c.name}.set(this, callback) }},");
                }
                sb.AppendLine($"    }}, {_g}.{obj.Key});");
            }
            sb.AppendLine($"    return createProxy({{");
            sb.AppendLine($"      onFrame(callback) {{ h.onFrame.set(this, callback) }},");
            sb.AppendLine($"      onReceive(callback) {{ h.onReceive.set(this, callback) }},");
            foreach (var obj in objs.Select(o => o.Key))
            {
                sb.AppendLine($"      get {obj}() {{ return _{obj} }},");
            }
            sb.AppendLine($"    }}, {_g});");
            sb.AppendLine($"  }}");
            sb.AppendLine($"}})();");

            return sb.ToString();
        }
    }
}
