using System;
using ClusterVR.CreatorKit;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEngine;

namespace Silksprite.PSCore.Access
{
    [Serializable]
    public sealed class ItemAudioSetListAccessEntry
    {
        [IdString] public string id;
        public AudioClip audioClip;
        public bool loop;
    }
}
