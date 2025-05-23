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

        JavaScriptGenerator(string g, string gg, CallbackDef[] callbackDefs)
        {
            _g = g;
            _gg = gg;
            _preamble = BuildPreamble(g, gg, callbackDefs);
        }

        public static JavaScriptGenerator ForItemScript()
        {
            const string g = "$";
            const string gg = "$$";
            var callbackDefs = new[]
            {
                new CallbackDef(null, "onCollide", null),
                new CallbackDef(null, "onCommentReceived", null),
                new CallbackDef(null, "onExternalCallEnd", null),
                new CallbackDef(null, "onGetOwnProducts", null),
                new CallbackDef(null, "onGiftSent", null),
                new CallbackDef(null, "onGrab", null),
                new CallbackDef(null, "onInteract", null),
                new CallbackDef(null, "onPhysicsUpdate", null),
                new CallbackDef(null, "onPurchaseUpdated", null),
                new CallbackDef(null, "onReceive", "@, { item: true, player: true }"),
                new CallbackDef(null, "onRequestGrantProductResult", null),
                new CallbackDef(null, "onRequestPurchaseStatus", null),
                new CallbackDef(null, "onRide", null),
                new CallbackDef(null, "onStart", null),
                new CallbackDef(null, "onSteer", null),
                new CallbackDef(null, "onSteerAdditionalAxis", null),
                new CallbackDef(null, "onTextInput", null),
                new CallbackDef(null, "onUpdate", null),
                new CallbackDef(null, "onUse", null),
            };
            return new JavaScriptGenerator(g, gg, callbackDefs);
        }

        public static JavaScriptGenerator ForPlayerScript()
        {
            const string g = "_";
            const string gg = "__";
            var callbackDefs = new CallbackDef[]
            {
                new CallbackDef(null, "onFrame", null),
                new CallbackDef(null, "onReceive", null),
                new CallbackDef("oscHandle", "onOscReceive", null),
            };
            return new JavaScriptGenerator(g, gg, callbackDefs);
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
        static string BuildPreamble(string g, string gg, CallbackDef[] callbackDefs)
        {
            var objs = callbackDefs
                .Where(c => c.Obj is not null)
                .GroupBy(c => c.Obj)
                .ToArray();
            var sb = new StringBuilder();
            
            sb.AppendLine($"const {gg} = (() => {{");
            sb.AppendLine($"  const x = new Proxy({{ }}, {{");
            sb.AppendLine($"    get: function(target, prop) {{ const v = !target[prop]; target[prop] = true; return v; }}");
            sb.AppendLine($"  }});");
            sb.AppendLine($"  const h = new Proxy({{ }}, {{");
            sb.AppendLine($"    get: function(target, prop) {{ return target[prop] ??= new Map(); }}");
            sb.AppendLine($"  }});");
            sb.AppendLine($"  const d = new Proxy({{ }}, {{");
            sb.AppendLine($"    get: function(target, prop) {{ return (...args) => {{ for (const f of h[prop].values()) f(...args); }}; }}");
            sb.AppendLine($"  }});");
            foreach (var c in callbackDefs)
            {
                sb.AppendLine($"  function {c.Name}(g, callback) {{");
                sb.AppendLine($"    if (x.{c.Name}) {g}.{c.Path}({c.Args.Replace("@", $"d.{c.Name}")});");
                sb.AppendLine($"    h.{c.Name}.set(g, callback);");
                sb.AppendLine($"  }}");
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
                    sb.AppendLine($"      {c.Name}(callback) {{ {c.Name}(this, callback) }},");
                }
                sb.AppendLine($"    }}, {g}.{obj.Key});");
            }
            sb.AppendLine($"    return createProxy({{");
            foreach (var c in callbackDefs.Where(c => c.Obj is null))
            {
                sb.AppendLine($"      {c.Name}(callback) {{ {c.Name}(this, callback) }},");
            }
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

    public readonly struct CallbackDef
    {
        public readonly string Obj;
        public readonly string Name;
        public readonly string Args;

        public string Path => Obj is null ? Name : $"{Obj}.{Name}";

        public CallbackDef(string obj, string name, string args)
        {
            Obj = obj;
            Name = name;
            Args = args ?? "@";
        }
    }
}
