using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class WorldItemReferenceListAccess : ObjectAccessBase<WorldItemReferenceList>
    {
        public WorldItemReferenceListAccess(WorldItemReferenceList worldItemReferenceList) : base(worldItemReferenceList)
        {
        }

        public void SetEntries(WorldItemReferenceListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("worldItemReferences"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("item").objectReferenceValue = entry.item;
            });
        }
    }
}
