using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public static class PlayerScriptMergerMenuItems
    {
        [MenuItem("Assets/Create/Silksprite/PSMerger/ClusterScriptAssetMerger", priority = 100)]
        static void CreateClusterScriptAssetMerger()
        {            
            var asset = ScriptableObject.CreateInstance<ClusterScriptAssetMerger>();
            ProjectWindowUtil.CreateAsset(asset, "ClusterScriptAssetMerger.asset");
        }
        
        [MenuItem("Assets/Create/Silksprite/PSMerger/ScriptMergerSource", priority = 200)]
        static void CreateScriptMergerSource()
        {            
            var asset = ScriptableObject.CreateInstance<ScriptMergerSource>();
            ProjectWindowUtil.CreateAsset(asset, "ScriptMergerSource.asset");
        }
    }
}
