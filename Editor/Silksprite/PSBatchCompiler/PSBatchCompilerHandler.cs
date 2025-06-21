using ClusterVR.CreatorKit.Editor.EditorEvents;
using Silksprite.PSCollector.Compiler;
using Silksprite.PSMerger.Compiler;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSBatchCompiler
{
    public static class PSBatchCompilerHandler
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
            PSMergerBatchCompiler.CompileAll();
            new PSCollectorBatchCompiler().CompileAll();
            Debug.Log($"[{PSMerger}]更新終了");
        }
    }
}
