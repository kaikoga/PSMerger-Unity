using System;
using ClusterVR.CreatorKit;
using UnityEngine;

namespace Silksprite.PSCore.Access
{
    [Serializable]
    public sealed class PlayerLocalObjectReferenceListAccessEntry
    {
        [IdString] public string id;
        public GameObject targetObject;
    }
}
