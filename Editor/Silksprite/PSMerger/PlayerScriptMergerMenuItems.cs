using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public static class PlayerScriptMergerMenuItems
    {
        [MenuItem("Assets/Create/Silksprite/ClusterScriptAssetMerger")]
        static void CreateClusterScriptAssetMerger()
        {            
            var asset = ScriptableObject.CreateInstance<ClusterScriptAssetMerger>();
            ProjectWindowUtil.CreateAsset(asset, "ClusterScriptAssetMerger.asset");
        }
    }
}
