using System;
using System.Linq;
using ClusterVR.CreatorKit.Editor.EditorEvents;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger.Compiler
{
    // CSCombinerと同じことをする
    public static class PlayerScriptMergerBatchCompiler
    {
        public static class EventHandler
        {
            [InitializeOnLoadMethod]
            public static void Initialize()
            {
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                WorldUploadEvents.RegisterOnWorldUploadStart(OnWorldUploadStarted);
            }

            static void OnPlayModeStateChanged(PlayModeStateChange playMode)
            {
                if (playMode == PlayModeStateChange.ExitingEditMode)
                {
                    CompileAll();
                }
            }

            static bool OnWorldUploadStarted(WorldUploadStartEventData data)
            {
                CompileAll();
                return true;
            }
        }

        static void CompileAll()
        {
            Debug.Log($"[{nameof(PlayerScriptMerger)}]更新開始");
            CombineAllOfScene();
            CombineAllOfProject();
            CombineAllAssets();
            Debug.Log($"[{nameof(PlayerScriptMerger)}]更新終了");
        }

        static void CombineAllOfScene()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var combiner in rootObjects.SelectMany(o => o.GetComponentsInChildren<PlayerScriptMerger>(true)))
            {
                Debug.Log($"[{nameof(PlayerScriptMerger)}][Scene]{combiner.name}");
                PlayerScriptMergerCompiler.Compile(combiner);
            }
        }

        static void CombineAllOfProject()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", null);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = PrefabUtility.LoadPrefabContents(path);

                var combiners = prefab.GetComponentsInChildren<PlayerScriptMerger>(true);
                if (combiners.Length == 0)
                {
                    PrefabUtility.UnloadPrefabContents(prefab);
                    continue;
                }

                Debug.Log($"[{nameof(PlayerScriptMerger)}][Prefab]{path}");

                var changed = false;
                foreach (var combiner in combiners)
                {
                    Debug.Log($"[{nameof(PlayerScriptMerger)}][Prefab]{combiner.name}");
                    changed |= PlayerScriptMergerCompiler.Compile(combiner);
                }

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                }

                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }

        static void CombineAllAssets()
        {
            var guids = AssetDatabase.FindAssets("t:ClusterScriptAssetMerger", null);
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var merger = AssetDatabase.LoadAssetAtPath<ClusterScriptAssetMerger>(path);
                switch (merger.ScriptType)
                {
                    case ClusterScriptType.ItemScript:
                        ItemScriptMergerCompiler.Compile(merger);
                        break;
                    case ClusterScriptType.PlayerScript:
                        PlayerScriptMergerCompiler.Compile(merger);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(merger.ScriptType), merger.ScriptType, null);
                }
            }
        }
    }
}
