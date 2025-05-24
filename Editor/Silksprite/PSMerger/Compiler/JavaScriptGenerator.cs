using System.Collections.Generic;
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
        readonly CallbackDef[] _callbackDefs;

        JavaScriptGenerator(string g, string gg, CallbackDef[] callbackDefs)
        {
            _g = g;
            _gg = gg;
            _callbackDefs = callbackDefs;
        }

        public static JavaScriptGenerator ForItemScript()
        {
            return new JavaScriptGenerator("$", "$$", new[]
            {
                new CallbackDef(null, "onCollide"),
                new CallbackDef(null, "onCommentReceived"),
                new CallbackDef(null, "onExternalCallEnd"),
                new CallbackDef(null, "onGetOwnProducts"),
                new CallbackDef(null, "onGiftSent"),
                new CallbackDef(null, "onGrab"),
                new CallbackDef(null, "onInteract"),
                new CallbackDef(null, "onPhysicsUpdate"),
                new CallbackDef(null, "onPurchaseUpdated"),
                new CallbackDef(null, "onReceive", "@, { item: true, player: true }"),
                new CallbackDef(null, "onRequestGrantProductResult"),
                new CallbackDef(null, "onRequestPurchaseStatus"),
                new CallbackDef(null, "onRide"),
                new CallbackDef(null, "onStart"),
                new CallbackDef(null, "onSteer"),
                new CallbackDef(null, "onSteerAdditionalAxis"),
                new CallbackDef(null, "onTextInput"),
                new CallbackDef(null, "onUpdate"),
                new CallbackDef(null, "onUse"),
            });
        }

        public static JavaScriptGenerator ForPlayerScript()
        {
            return new JavaScriptGenerator("_", "__", new[]
            {
                new CallbackDef(null, "onFrame"),
                new CallbackDef(null, "onReceive"),
                new CallbackDef("oscHandle", "onOscReceive"),
                new CallbackDef(null, "onButton0", "0, @", "onButton"),
                new CallbackDef(null, "onButton1", "1, @", "onButton"),
                new CallbackDef(null, "onButton2", "2, @", "onButton"),
                new CallbackDef(null, "onButton3", "3, @", "onButton"),
            });
        }

        public string MergeScripts(JavaScriptSource javaScriptSource)
        {
            var scripts = javaScriptSource.ScriptContexts;
            var allScripts = javaScriptSource.AllScripts;
            var callbackDefs = _callbackDefs
                .Where(def => allScripts.Any(s => s.text.Contains(def.ApiName)))
                .ToArray();
            var preamble = BuildPreamble(_g, _gg, callbackDefs);
            return preamble + string.Join("\n", scripts.Select(context => $@"
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
            var rootCallbackDefs = callbackDefs.Where(c => c.Obj is null);
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
                foreach (var apiBody in obj.Select(o => o.ApiBody).Distinct())
                {
                    sb.AppendLine($"      {apiBody},");
                }
                sb.AppendLine($"    }}, {g}.{obj.Key});");
            }
            sb.AppendLine($"    return createProxy({{");
            foreach (var apiBody in rootCallbackDefs.Select(o => o.ApiBody).Distinct())
            {
                sb.AppendLine($"      {apiBody},");
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
        public readonly string Path;
        public readonly string ApiName;
        public readonly string ApiBody;

        public CallbackDef(string obj, string name, string args = null, string apiName = null)
        {
            const string onButtonBody = @"onButton(index, callback) {
        switch (index) {
          case 0: onButton0(this, callback); break;
          case 1: onButton1(this, callback); break;
          case 2: onButton2(this, callback); break;
          case 3: onButton3(this, callback); break;
        }
      }";
            Obj = obj;
            Name = name;
            Args = args ?? "@";
            ApiName = apiName ?? name;
            Path = Obj is null ? ApiName : $"{Obj}.{ApiName}";
            ApiBody = ApiName == "onButton" ? onButtonBody :  $"{ApiName}(callback) {{ {ApiName}(this, callback) }}";
        }
    }
}
