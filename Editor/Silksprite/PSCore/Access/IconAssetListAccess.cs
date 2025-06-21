using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class IconAssetListAccess : ObjectAccessBase<IconAssetList>
    {
        public IconAssetListAccess(IconAssetList iconAssetList) : base(iconAssetList)
        {
        }

        public void SetEntries(IconAssetListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("iconAssets"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("image").objectReferenceValue = entry.image;
            });
        }
    }
}
