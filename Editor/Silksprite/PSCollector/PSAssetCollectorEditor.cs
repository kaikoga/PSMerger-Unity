using ClusterVR.CreatorKit.Editor.Custom;
using Silksprite.PSCollector.Compiler;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Silksprite.PSCollector
{
    [CustomEditor(typeof(PSAssetCollector)), CanEditMultipleObjects]
    public class PSAssetCollectorEditor : VisualElementEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var content = base.CreateInspectorGUI();
            content.Add(new IMGUIContainer(() =>
            {
                if (GUILayout.Button("Collect"))
                {
                    foreach (var t in targets)
                    {
                        PSAssetCollectorCompiler.Collect((PSAssetCollector) t);
                    }
                }
            }));
            return content;
        }
    }
}
