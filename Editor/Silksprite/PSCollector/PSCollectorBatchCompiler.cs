using System.Linq;
using UnityEngine;

namespace Silksprite.PSCollector.Compiler
{
    // CSCombinerと同じことをする
    public class PSCollectorBatchCompiler
    {
        const string PSMerger = "PSCollector";

        readonly PSAssetCollectorProcessor _processor = new();

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
                Debug.Log($"[{mergerComponent.GetType().Name}][Scene]{mergerComponent.gameObject.name}");
                CollectSingleComponent(mergerComponent);
            }
        }

        bool CollectSingleComponent(PSAssetCollector collectorComponent)
        {
            return _processor.Collect(collectorComponent);
        }
    }
}
