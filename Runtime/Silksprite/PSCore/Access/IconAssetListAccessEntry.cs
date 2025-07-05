using System;
using ClusterVR.CreatorKit;
using UnityEngine;

namespace Silksprite.PSCore.Access
{
    [Serializable]
    public sealed class IconAssetListAccessEntry
    {
        [IdString] public string id;
        public Texture2D image;
    }
}
