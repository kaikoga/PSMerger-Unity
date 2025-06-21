using System;
using ClusterVR.CreatorKit;
using ClusterVR.CreatorKit.Item.Implements;

namespace Silksprite.PSCore.Access
{
    [Serializable]
    public sealed class WorldItemTemplateListAccessEntry
    {
        [IdString] public string id;
        public Item worldItemTemplate;
    }
}
