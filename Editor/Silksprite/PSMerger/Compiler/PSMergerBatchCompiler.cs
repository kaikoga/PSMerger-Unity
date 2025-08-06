using System;
using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger.Compiler
{
    // CSCombinerと同じことをする
    public class PSMergerBatchCompiler
    {
        const string PSMerger = "PSMerger";

        readonly HashSet<JavaScriptAsset> _mergedScriptAssets = new();

        enum Result
        {
            NoProcess,
            Skipped,
            Updated,
            NoChange,
        }

        bool WillProcessAsset(JavaScriptAsset javaScriptAsset, bool allowNull, out Result result)
        {
            if (!(allowNull || javaScriptAsset))
            {
                result = Result.NoProcess;
                return false;
            }
            if (!_mergedScriptAssets.Add(javaScriptAsset))
            {
                result = Result.Skipped;
                return false;
            }
            result = Result.NoChange;
            return true;
        }

        public void CompileAll()
        {
            CombineAllOfScene();
            CombineAllOfProject();
            CombineAllAssets();
        }

        void CombineAllOfScene()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var mergerComponent in rootObjects.SelectMany(o => o.GetComponentsInChildren<ClusterScriptComponentMergerBase>(true)))
            {
                if (WillProcessAsset(mergerComponent.MergedScript, true, out var result))
                {
                    result = CombineSingleComponent(mergerComponent) ? Result.Updated : Result.NoChange;
                }
                Debug.Log($"[{PSMerger}[Scene][{mergerComponent.GetType().Name}]{mergerComponent.gameObject.name} {result}", mergerComponent);
            }
        }

        void CombineAllOfProject()
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

                Debug.Log($"[{PSMerger}][Prefab]{path}", prefab);

                var changed = false;
                foreach (var mergerComponent in mergerComponents)
                {
                    if (WillProcessAsset(mergerComponent.MergedScript, true, out var result))
                    {
                        changed |= CombineSingleComponent(mergerComponent);
                    }
                    Debug.Log($"[{PSMerger}][Prefab][{mergerComponent.GetType().Name}]{mergerComponent.gameObject.name} {result}", prefab);
                }

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                }

                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }

        bool CombineSingleComponent(ClusterScriptComponentMergerBase mergerComponent)
        {
            return mergerComponent switch
            {
                PlayerScriptMerger playerScriptMerger => PlayerScriptMergerCompiler.Compile(playerScriptMerger),
                ItemScriptMerger itemScriptMerger => ItemScriptMergerCompiler.Compile(itemScriptMerger),
                _ => throw new ArgumentException($"{mergerComponent.GetType().Name} is not supported")
            };
        }

        void CombineAllAssets()
        {
            var guids = AssetDatabase.FindAssets("t:ClusterScriptAssetMerger", null);
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var merger = AssetDatabase.LoadAssetAtPath<ClusterScriptAssetMerger>(path);
                if (WillProcessAsset(merger.MergedScript, false, out var result))
                {
                    result = ClusterScriptAssetMergerCompiler.Compile(merger) ? Result.Updated : Result.NoChange;
                }
                Debug.Log($"[{PSMerger}][Asset][{merger.GetType().Name}]{merger.name} {result}", merger);
            }
        }
    }
}
