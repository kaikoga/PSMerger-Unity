using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access.Base;

namespace Silksprite.PSCore.Access
{
    public class HumanoidAnimationListAccess : ObjectAccessBase<HumanoidAnimationList>
    {
        public HumanoidAnimationListAccess(HumanoidAnimationList humanoidAnimationList) : base(humanoidAnimationList)
        {
        }

        public void SetEntries(HumanoidAnimationListAccessEntry[] entries)
        {
            AccessEntryListAccess.SetEntries(serializedObject.FindProperty("humanoidAnimations"), entries, (entryProperty, entry) =>
            {
                entryProperty.FindPropertyRelative("id").stringValue = entry.id;
                entryProperty.FindPropertyRelative("animation").objectReferenceValue = entry.animation;
            });
        }
    }
}
