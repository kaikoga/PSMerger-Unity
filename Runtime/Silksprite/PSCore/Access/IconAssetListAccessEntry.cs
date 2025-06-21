using System;
using ClusterVR.CreatorKit;
using ClusterVR.CreatorKit.Item.Implements;
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
