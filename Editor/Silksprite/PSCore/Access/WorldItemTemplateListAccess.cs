using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class WorldItemTemplateListAccess : ObjectAccessBase<WorldItemTemplateList>
    {
        public WorldItemTemplateListAccess(WorldItemTemplateList worldItemTemplateList) : base(worldItemTemplateList)
        {
        }

        public void SetEntries(WorldItemTemplateListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("worldItemTemplates"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("worldItemTemplate").objectReferenceValue = entry.worldItemTemplate;
            });
        }
    }
}
