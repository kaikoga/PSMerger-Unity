using System.Collections.Generic;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSMerger.Compiler.Data;
using Silksprite.PSMerger.Compiler.Extension;
using Silksprite.PSMerger.Compiler.Filter;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public class ClusterScriptAssetMerger : ScriptableObject
    {
        public const string NameofJavaScriptSource = nameof(javaScriptSource);
        public const string NameofOtherSources = nameof(otherSources);
        public const string NameofMergedScript = nameof(mergedScript);
        public const string NameofDetectCallbackSupport = nameof(detectCallbackSupport);
        public const string NameofGenerateSourcemap = nameof(generateSourcemap);

        [SerializeField] ClusterScriptType scriptType = ClusterScriptType.PlayerScript;
        [SerializeField] JavaScriptSource javaScriptSource;
        [SerializeField] ScriptMergerSource[] otherSources = { };
        [SerializeField] JavaScriptAsset mergedScript;
        [SerializeField] bool detectCallbackSupport = true;
        [SerializeField] bool generateSourcemap;
        
        public ClusterScriptType ScriptType => scriptType;
        public JavaScriptAsset MergedScript => mergedScript;
        public bool DetectCallbackSupport => detectCallbackSupport;
        public bool GenerateSourcemap => generateSourcemap;

        public IEnumerable<JavaScriptSource> JavaScriptSources()
        {
            yield return javaScriptSource;
            foreach (var otherSource in otherSources) yield return otherSource?.JavaScriptSource;
        }

        public void SetMergedScript(JavaScriptAsset javaScriptAsset)
        {
            mergedScript = javaScriptAsset;
        }

        public JavaScriptCompilerEnvironment ToCompilerEnvironment()
        {
            var environment = JavaScriptCompilerEnvironmentFactory.Create(
                JavaScriptSources(),
                DetectCallbackSupport,
                null);
            return PSMergerFilter.Apply(environment, this);
        }
    }

    public enum ClusterScriptType
    {
        ConcatOnly,
        ItemScript,
        PlayerScript
    }
}
