using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Silksprite.PSMerger.Compiler.Internal
{
    public class MergedJavaScriptGenerator
    {
        readonly string _g;
        readonly string _gg;
        readonly CallbackDef[] _callbackDefs;

        MergedJavaScriptGenerator(string g, string gg, CallbackDef[] callbackDefs)
        {
            _g = g;
            _gg = gg;
            _callbackDefs = callbackDefs;
        }

        public static MergedJavaScriptGenerator ForItemScript()
        {
            return new MergedJavaScriptGenerator("$", "$$", new[]
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

        public static MergedJavaScriptGenerator ForPlayerScript()
        {
            return new MergedJavaScriptGenerator("_", "__", new[]
            {
                new CallbackDef(null, "onButton0", "0, @", "onButton"),
                new CallbackDef(null, "onButton1", "1, @", "onButton"),
                new CallbackDef(null, "onButton2", "2, @", "onButton"),
                new CallbackDef(null, "onButton3", "3, @", "onButton"),
                new CallbackDef(null, "onFrame"),
                new CallbackDef(null, "onReceive"),
                new CallbackDef("oscHandle", "onOscReceive"),
            });
        }

        public JavaScriptCompilerOutput MergeScripts(JavaScriptCompilerEnvironment env)
        {
            var output = new JavaScriptCompilerOutput(env.OutputFileName);
            var allScripts = env.AllInputs().Select(input => input.Text).ToArray();
            var callbackDefs = env.DetectCallbackSupport
                ? _callbackDefs
                    .Where(def => allScripts.Any(script => script.Contains(def.ApiName)))
                    .ToArray()
                : _callbackDefs;
            var scriptContexts = env.ScriptContexts;
            foreach (var lib in env.ScriptLibraries)
            {
                output.AppendLine(lib.Text);
            }
            if (env.DetectCallbackSupport || scriptContexts.Length > 0)
            {
                BuildPreamble(_g, _gg, callbackDefs, output);
                foreach (var context in scriptContexts)
                {
                    output.AppendLine($"({_g} => {{");
                    foreach (var input in context.JavaScriptInputs)
                    {
                        output.AppendInput(input);
                    }
                    output.AppendLine($"}})({_gg}());");
                }
            }
            return output;
        }

        [SuppressMessage("ReSharper", "RedundantStringInterpolation")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        static void BuildPreamble(string g, string gg, CallbackDef[] callbackDefs, JavaScriptCompilerOutput output)
        {
            var objs = callbackDefs
                .Where(c => c.Obj is not null)
                .GroupBy(c => c.Obj)
                .ToArray();
            var rootCallbackDefs = callbackDefs.Where(c => c.Obj is null);
            output.AppendLine($"const {gg} = (() => {{");
            output.AppendLine($"  const x = new Proxy({{ }}, {{");
            output.AppendLine($"    get: function(target, prop) {{ const v = !target[prop]; target[prop] = true; return v; }}");
            output.AppendLine($"  }});");
            output.AppendLine($"  const h = new Proxy({{ }}, {{");
            output.AppendLine($"    get: function(target, prop) {{ return target[prop] ??= new Map(); }}");
            output.AppendLine($"  }});");
            output.AppendLine($"  const d = new Proxy({{ }}, {{");
            output.AppendLine($"    get: function(target, prop) {{ return (...args) => {{ for (const f of h[prop].values()) f(...args); }}; }}");
            output.AppendLine($"  }});");
            foreach (var c in callbackDefs)
            {
                output.AppendLine($"  function {c.Name}(g, callback) {{");
                output.AppendLine($"    if (x.{c.Name}) {g}.{c.Path}({c.Args.Replace("@", $"d.{c.Name}")});");
                output.AppendLine($"    h.{c.Name}.set(g, callback);");
                output.AppendLine($"  }}");
            }
            output.AppendLine($"  function createProxy(obj, base) {{");
            output.AppendLine($"    return new Proxy(obj, {{");
            output.AppendLine($"      get(target, prop, receiver) {{");
            output.AppendLine($"        let thisValue = target; ");
            output.AppendLine($"        let value = target[prop];");
            output.AppendLine($"        if (value === undefined) {{");
            output.AppendLine($"          thisValue = base;");
            output.AppendLine($"          value = base[prop];");
            output.AppendLine($"        }}");
            output.AppendLine($"        return (value instanceof Function) ? value.bind(thisValue) : value;");
            output.AppendLine($"      }}");
            output.AppendLine($"    }});");
            output.AppendLine($"  }}");
            output.AppendLine($"  return () => {{");
            foreach (var obj in objs)
            {
                output.AppendLine($"    const _{obj.Key} = createProxy({{");
                foreach (var apiBody in obj.Select(o => o.ApiBody).Distinct())
                {
                    output.AppendLines($"      {apiBody},");
                }
                output.AppendLine($"    }}, {g}.{obj.Key});");
            }
            output.AppendLine($"    return createProxy({{");
            foreach (var apiBody in rootCallbackDefs.Select(o => o.ApiBody).Distinct())
            {
                output.AppendLines($"      {apiBody},");
            }
            foreach (var obj in objs.Select(o => o.Key))
            {
                output.AppendLine($"      get {obj}() {{ return _{obj} }},");
            }
            output.AppendLine($"    }}, {g});");
            output.AppendLine($"  }}");
            output.AppendLine($"}})();");
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
