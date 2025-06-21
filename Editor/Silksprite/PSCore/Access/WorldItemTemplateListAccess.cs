using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WorldItemTemplateListAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
        
        public WorldItemTemplateListAccess(WorldItemTemplateList worldItemTemplateList)
        {
            _serializedObject = new SerializedObject(worldItemTemplateList);
        }

        public void SetEntries(IEnumerable<WorldItemTemplateListAccessEntry> entries)
        {
            var e = entries.ToArray();
            var referencesProperty = _serializedObject.FindProperty("worldItemTemplates");
            referencesProperty.ClearArray();
            referencesProperty.arraySize = e.Length;
            for (var i = 0; i < e.Length; i++)
            {
                var entry = e[i];
                var entryProperty = referencesProperty.GetArrayElementAtIndex(i);
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("worldItemTemplate").objectReferenceValue = entry.worldItemTemplate;
            }
        }

        public void Dispose()
        {
            _serializedObject.ApplyModifiedProperties();
            _serializedObject?.Dispose();
        }
    }
        
    [Serializable]
    public sealed class WorldItemTemplateListAccessEntry
    {
        public string id;
        public Item worldItemTemplate;
    }
}
