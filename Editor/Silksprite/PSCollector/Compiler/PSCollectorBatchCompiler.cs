using System.Linq;
using UnityEngine;

namespace Silksprite.PSCollector.Compiler
{
    // CSCombinerと同じことをする
    public class PSCollectorBatchCompiler
    {
        const string PSCollector = "PSCollector";

        public void CompileAll()
        {
            CollectAllOfScene();
        }

        void CollectAllOfScene()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var mergerComponent in rootObjects.SelectMany(o => o.GetComponentsInChildren<PSAssetCollector>(true)))
            {
                Debug.Log($"[{PSCollector}][Scene][{mergerComponent.GetType().Name}]{mergerComponent.gameObject.name}", mergerComponent);
                CollectSingleComponent(mergerComponent);
            }
        }

        bool CollectSingleComponent(PSAssetCollector collectorComponent)
        {
            return PSAssetCollectorCompiler.Collect(collectorComponent);
        }
    }
}
