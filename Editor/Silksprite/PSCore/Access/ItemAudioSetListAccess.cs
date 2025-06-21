using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ItemAudioSetListAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
        
        public ItemAudioSetListAccess(ItemAudioSetList itemAudioSetList)
        {
            _serializedObject = new SerializedObject(itemAudioSetList);
        }

        public void SetEntries(IEnumerable<ItemAudioSetListAccessEntry> entries)
        {
            var e = entries.ToArray();
            var referencesProperty = _serializedObject.FindProperty("itemAudioSets");
            referencesProperty.ClearArray();
            referencesProperty.arraySize = e.Length;
            for (var i = 0; i < e.Length; i++)
            {
                var entry = e[i];
                var entryProperty = referencesProperty.GetArrayElementAtIndex(i);
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("audioClip").objectReferenceValue = entry.audioClip;
                entryProperty.FindPropertyRelative("loop").boolValue = entry.loop;
            }
        }

        public void Dispose()
        {
            _serializedObject.ApplyModifiedProperties();
            _serializedObject?.Dispose();
        }
    }
}
