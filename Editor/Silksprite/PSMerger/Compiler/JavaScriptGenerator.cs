using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSMerger.Compiler
{
    public class JavaScriptGenerator
    {
        readonly string _g;
        readonly string _gg;
        readonly string _preamble;

        public JavaScriptGenerator(bool playerScript)
        {
            _g = playerScript ? "_" : "$";
            _gg = playerScript ? "__" : "$$";
            _preamble = playerScript 
                ? BuildPreamble(_g, _gg, new (string, string, string)[]
                {
                    (null, "onFrame", null),
                    (null, "onReceive", null),
                    ("oscHandle", "onOscReceive", null)
                })
                : BuildPreamble(_g, _gg, new (string, string, string)[]
                {
                    (null, "onUpdate", null),
                    (null, "onReceive", "{ item: true, player: true }")
                });
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
        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        static string BuildPreamble(string g, string gg, (string obj, string name, string args)[] callbackDefs)
        {
            var callbacks = callbackDefs
                .Select(def => (def.obj, def.name, path: def.obj is null ? def.name : $"{def.obj}.{def.name}", def.args))
                .ToArray();
            var objs = callbacks
                .Where(c => c.obj is not null)
                .GroupBy(c => c.obj)
                .ToArray();
            var sb = new StringBuilder();
            
            sb.AppendLine($"const {gg} = (() => {{");
            sb.AppendLine($"  const h = new Proxy(new Map(), {{");
            sb.AppendLine($"    get: function(target, prop) {{ return target.has(prop) ? target.get(prop) : target.set(prop, new Map()).get(prop); }}");
            sb.AppendLine($"  }});");
            foreach (var callback in callbacks)
            {
                sb.AppendLine($"  {g}.{callback.path}((...args) => {{");
                sb.AppendLine($"    for (const f of h.{callback.name}.values()) f(...args);");
                if (callback.args is null)
                {
                    sb.AppendLine($"  }});");
                }
                else
                {
                    sb.AppendLine($"  }}, {callback.args});");
                }
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
                sb.AppendLine($"    }}, {g}.{obj.Key});");
            }
            sb.AppendLine($"    return createProxy({{");
            sb.AppendLine($"      onFrame(callback) {{ h.onFrame.set(this, callback) }},");
            sb.AppendLine($"      onReceive(callback) {{ h.onReceive.set(this, callback) }},");
            foreach (var obj in objs.Select(o => o.Key))
            {
                sb.AppendLine($"      get {obj}() {{ return _{obj} }},");
            }
            sb.AppendLine($"    }}, {g});");
            sb.AppendLine($"  }}");
            sb.AppendLine($"}})();");

            return sb.ToString();
        }
    }
}
