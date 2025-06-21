using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WorldItemReferenceListAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
        
        public WorldItemReferenceListAccess(WorldItemReferenceList worldItemReferenceList)
        {
            _serializedObject = new SerializedObject(worldItemReferenceList);
        }

        public void SetEntries(IEnumerable<WorldItemReferenceListAccessEntry> entries)
        {
            var e = entries.ToArray();
            var referencesProperty = _serializedObject.FindProperty("worldItemReferences");
            referencesProperty.ClearArray();
            referencesProperty.arraySize = e.Length;
            for (var i = 0; i < e.Length; i++)
            {
                var entry = e[i];
                var entryProperty = referencesProperty.GetArrayElementAtIndex(i);
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("item").objectReferenceValue = entry.item;
            }
        }

        public void Dispose()
        {
            _serializedObject.ApplyModifiedProperties();
            _serializedObject?.Dispose();
        }
    }
        
    [Serializable]
    public sealed class WorldItemReferenceListAccessEntry
    {
        public string id;
        public Item item;
    }
}
