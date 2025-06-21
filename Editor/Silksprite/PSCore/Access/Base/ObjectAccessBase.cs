using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Silksprite.PSCore.Access.Base
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class ObjectAccessBase<T> : IDisposable
    where T : Object
    {
        protected readonly SerializedObject serializedObject;

        public bool hasModifiedProperties => serializedObject.hasModifiedProperties;

        protected ObjectAccessBase(T obj) => serializedObject = new SerializedObject(obj);

        public virtual void Dispose()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject?.Dispose();
        }
    }
}
