using System;
using UnityEditor;

namespace Silksprite.PSCore.Access.Base
{
    public static class AccessEntryListAccess
    {
        public static void SetEntries<T>(SerializedProperty targetProperty, T[] entries, Action<SerializedProperty, T> writer)
        {
            targetProperty.ClearArray();
            targetProperty.arraySize = entries.Length;
            for (var i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];
                var entryProperty = targetProperty.GetArrayElementAtIndex(i);
                writer(entryProperty, entry);
            }
        }
    }
}
