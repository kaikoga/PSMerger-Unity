using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class ItemAudioSetListAccess : ObjectAccessBase<ItemAudioSetList>
    {
        public ItemAudioSetListAccess(ItemAudioSetList itemAudioSetList) : base(itemAudioSetList)
        {
        }

        public void SetEntries(ItemAudioSetListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("itemAudioSets"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("audioClip").objectReferenceValue = entry.audioClip;
                entryProperty.FindPropertyRelative("loop").boolValue = entry.loop;
            });
        }
    }
}
