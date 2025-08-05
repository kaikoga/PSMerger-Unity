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
                var skipped = mergerComponent.MergedScript && !_mergedScriptAssets.Add(mergerComponent.MergedScript);
                Debug.Log($"[{PSMerger}[Scene][{mergerComponent.GetType().Name}]{mergerComponent.gameObject.name}{(skipped ? " Skipped" : "")}", mergerComponent);
                if (!skipped)
                {
                    CombineSingleComponent(mergerComponent);
                }
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
                    var skipped = mergerComponent.MergedScript && !_mergedScriptAssets.Add(mergerComponent.MergedScript);
                    Debug.Log($"[{PSMerger}][Prefab][{mergerComponent.GetType().Name}]{mergerComponent.gameObject.name}{(skipped ? " Skipped" : "")}", prefab);
                    if (!skipped)
                    {
                        changed |= CombineSingleComponent(mergerComponent);
                    }
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
                var skipped = merger.MergedScript && !_mergedScriptAssets.Add(merger.MergedScript);
                Debug.Log($"[{PSMerger}][Asset][{merger.GetType().Name}]{merger.name}{(skipped ? " Skipped" : "")}", merger);
                if (!skipped)
                {
                    ClusterScriptAssetMergerCompiler.Compile(merger);
                }
            }
        }
    }
}
