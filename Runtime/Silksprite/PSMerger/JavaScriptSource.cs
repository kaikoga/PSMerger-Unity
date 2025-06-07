using System;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSMerger
{
    [Serializable]
    public class JavaScriptSource
    {
        [SerializeField] internal JavaScriptAsset[] scriptLibraries = { };
        [SerializeField] internal JavaScriptContext[] scriptContexts = { };
        [SerializeField] internal bool detectCallbackSupport = true;
    }
}
