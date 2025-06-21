using System;
using System.Linq;
using ClusterVR.CreatorKit.Editor.EditorEvents;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger.Compiler
{
    // CSCombinerと同じことをする
    public static class PSMergerBatchCompiler
    {
        const string PSMerger = "PSMerger";

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
            Debug.Log($"[{PSMerger}]更新開始");
            CombineAllAssets();
            CombineAllOfScene();
            CombineAllOfProject();
            Debug.Log($"[{PSMerger}]更新終了");
        }

        static void CombineAllOfScene()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var mergerComponent in rootObjects.SelectMany(o => o.GetComponentsInChildren<ClusterScriptComponentMergerBase>(true)))
            {
                Debug.Log($"[{mergerComponent.GetType().Name}][Scene]{mergerComponent.gameObject.name}");
                CombineSingleComponent(mergerComponent);
            }
        }

        static void CombineAllOfProject()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", null);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = PrefabUtility.LoadPrefabContents(path);

                var mergerComponents = prefab.GetComponentsInChildren<ClusterScriptComponentMergerBase>(true);
                if (mergerComponents.Length == 0)
                {
                    PrefabUtility.UnloadPrefabContents(prefab);
                    continue;
                }

                Debug.Log($"[{PSMerger}][Prefab]{path}");

                var changed = false;
                foreach (var mergerComponent in mergerComponents)
                {
                    Debug.Log($"[{mergerComponent.GetType().Name}][Prefab]{mergerComponent.gameObject.name}");
                    changed |= CombineSingleComponent(mergerComponent);
                }

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                }

                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }

        static bool CombineSingleComponent(ClusterScriptComponentMergerBase mergerComponent)
        {
            return mergerComponent switch
            {
                PlayerScriptMerger playerScriptMerger => PlayerScriptMergerCompiler.Compile(playerScriptMerger),
                ItemScriptMerger itemScriptMerger => ItemScriptMergerCompiler.Compile(itemScriptMerger),
                _ => throw new ArgumentException($"{mergerComponent.GetType().Name} is not supported")
            };
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
                    case ClusterScriptType.ConcatOnly:
                        ConcatOnlyCompiler.Compile(merger);
                        break;
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
