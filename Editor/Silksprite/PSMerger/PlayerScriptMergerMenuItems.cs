using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public static class PlayerScriptMergerMenuItems
    {
        [MenuItem("Assets/Create/Silksprite/PlayerScriptAssetMerger")]
        static void CreatePlayerScriptAssetMerger()
        {            
            var asset = ScriptableObject.CreateInstance<PlayerScriptAssetMerger>();
            ProjectWindowUtil.CreateAsset(asset, "PlayerScriptAssetMerger.asset");
        }
    }
}
