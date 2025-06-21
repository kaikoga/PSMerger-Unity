using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class PlayerLocalObjectReferenceListAccess : ObjectAccessBase<PlayerLocalObjectReferenceList>
    {
        public PlayerLocalObjectReferenceListAccess(PlayerLocalObjectReferenceList playerLocalObjectReferenceList) : base(playerLocalObjectReferenceList)
        {
        }

        public void SetEntries(PlayerLocalObjectReferenceListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("playerLocalObjectReferences"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("targetObject").objectReferenceValue = entry.targetObject;
            });
        }
    }
}
