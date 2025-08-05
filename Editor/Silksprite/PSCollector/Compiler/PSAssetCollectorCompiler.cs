using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector.Compiler
{
    public static class PSAssetCollectorCompiler
    {
        static bool CollectEntries<TEntry>(out TEntry[] entries)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            entries = rootObjects.SelectMany(o => o.GetComponentsInChildren<IMergedAccessEntryList<TEntry>>(true))
                .SelectMany(entryList => entryList.Entries)
                .ToArray();
            return entries.Length > 0;
        }

        public static bool Collect(PSAssetCollector collector)
        {
            var changed = false;
            changed |= CollectItemAudioSetLists(collector);
            changed |= CollectHumanoidAnimationLists(collector);
            changed |= CollectWorldItemReferenceLists(collector);
            changed |= CollectWorldItemTemplateLists(collector);
            changed |= CollectPlayerLocalObjectReferenceLists(collector);
            changed |= CollectIconAssetLists(collector);
            return changed;
        }

        static bool CollectItemAudioSetLists(PSAssetCollector collector)
        {
            if (!CollectEntries<ItemAudioSetListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new ItemAudioSetListAccess(GetOrAddComponent<ItemAudioSetList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static bool CollectHumanoidAnimationLists(PSAssetCollector collector)
        {
            if (!CollectEntries<HumanoidAnimationListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new HumanoidAnimationListAccess(GetOrAddComponent<HumanoidAnimationList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static bool CollectWorldItemReferenceLists(PSAssetCollector collector)
        {
            if (!CollectEntries<WorldItemReferenceListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new WorldItemReferenceListAccess(GetOrAddComponent<WorldItemReferenceList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static bool CollectWorldItemTemplateLists(PSAssetCollector collector)
        {
            if (!CollectEntries<WorldItemTemplateListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new WorldItemTemplateListAccess(GetOrAddComponent<WorldItemTemplateList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static bool CollectPlayerLocalObjectReferenceLists(PSAssetCollector collector)
        {
            if (!CollectEntries<PlayerLocalObjectReferenceListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new PlayerLocalObjectReferenceListAccess(GetOrAddComponent<PlayerLocalObjectReferenceList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static bool CollectIconAssetLists(PSAssetCollector collector)
        {
            if (!CollectEntries<IconAssetListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new IconAssetListAccess(GetOrAddComponent<IconAssetList>(collector));
            access.SetEntries(entries);
            return true;
        }

        static T GetOrAddComponent<T>(PSAssetCollector collector)
            where T : Component
        {
            if (!collector.gameObject.TryGetComponent<T>(out var component))
            {
                component = collector.gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}
