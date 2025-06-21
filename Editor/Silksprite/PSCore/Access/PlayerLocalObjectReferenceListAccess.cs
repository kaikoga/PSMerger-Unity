using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSCore.Access
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PlayerLocalObjectReferenceListAccess : IDisposable
    {
        readonly SerializedObject _serializedObject;
        
        public PlayerLocalObjectReferenceListAccess(PlayerLocalObjectReferenceList playerLocalObjectReferenceList)
        {
            _serializedObject = new SerializedObject(playerLocalObjectReferenceList);
        }

        public void SetEntries(IEnumerable<PlayerLocalObjectReferenceListEntry> entries)
        {
            var e = entries.ToArray();
            var referencesProperty = _serializedObject.FindProperty("playerLocalObjectReferences");
            referencesProperty.ClearArray();
            referencesProperty.arraySize = e.Length;
            for (var i = 0; i < e.Length; i++)
            {
                var entry = e[i];
                var entryProperty = referencesProperty.GetArrayElementAtIndex(i);
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("targetObject").objectReferenceValue = entry.targetObject;
            }
        }

        public void Dispose()
        {
            _serializedObject.ApplyModifiedPropertiesWithoutUndo();
            _serializedObject?.Dispose();
        }
    }
        
    [Serializable]
    public sealed class PlayerLocalObjectReferenceListEntry
    {
        public string id;
        public GameObject targetObject;
    }
}
