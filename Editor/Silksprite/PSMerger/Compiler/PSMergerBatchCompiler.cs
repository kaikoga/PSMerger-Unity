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
            CombineAllAssets();
            CombineAllOfScene();
            CombineAllOfProject();
        }

        void CombineAllOfScene()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var mergerComponent in rootObjects.SelectMany(o => o.GetComponentsInChildren<ClusterScriptComponentMergerBase>(true)))
            {
                Debug.Log($"[{mergerComponent.GetType().Name}][Scene]{mergerComponent.gameObject.name}");
                CombineSingleComponent(mergerComponent);
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

        bool CombineSingleComponent(ClusterScriptComponentMergerBase mergerComponent)
        {
            if (!_mergedScriptAssets.Add(mergerComponent.MergedScript))
            {
                return false;
            }
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
                if (!_mergedScriptAssets.Add(merger.MergedScript))
                {
                    continue;
                }

                ClusterScriptAssetMergerCompiler.Compile(merger);
            }
        }
    }
}
