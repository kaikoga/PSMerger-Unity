using System;
using System.Collections.Generic;
using System.Linq;
using Baxter.ClusterScriptExtensions;
using Baxter.ClusterScriptExtensions.Editor.ScriptParser;
using Silksprite.PSMerger.Compiler.Internal;

namespace Silksprite.PSMerger.CSExEx
{
    public static class FieldReloadUtil
    {
        static IEnumerable<JavaScriptSource> CollectMergedSources()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            return rootObjects.SelectMany(o => o.GetComponentsInChildren<IMergedPlayerScriptSource>(true))
                .SelectMany(mergedSource => mergedSource.JavaScriptSources());
        }

        public static void ReloadFields(ClusterScriptComponentMergerExtensionBase ext, bool refresh)
        {
            var env = ext.MergerBase.ToCompilerEnvironment(CollectMergedSources());
                
            var templateCodes = env.AllInputs().Select(input => input.SourceCode).ToArray();
            if (templateCodes.Length == 0)
            {
                ext.SetFields(Array.Empty<ScriptExtensionField>());
            }
            else
            {
                var fields = templateCodes.SelectMany(ExtensionFieldParser.ExtractTargetFields).ToArray();
                foreach (var f in fields)
                {
                    InitializeExtensionFieldValue(f, ext.ExtensionFields, refresh);
                }
                ext.SetFields(fields);
            }
        }
        
        private static void InitializeExtensionFieldValue(
            ScriptExtensionField field, ScriptExtensionField[] existingFields, bool refresh)
        {
            if (refresh)
            {
                field.ResetValues();
                return;
            }

            // - 名前と型が同じフィールドの値があれば持ち越す
            // - そうでない場合、スクリプトを参考に初期値が定まる
            var existingField = existingFields.FirstOrDefault(
                ef => ef.FieldName == field.FieldName && ef.Type == field.Type
            );
            
            if (existingField != null)
            {
                field.CopyValues(existingField);
            }
            else
            {
                field.ResetValues();
            }
        }
    }
}
