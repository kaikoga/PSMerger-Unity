using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector.Compiler
{
    public class PSAssetCollectorCompiler
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

        public bool Collect(PSAssetCollector collector)
        {
            var changed = false;
            changed |= CollectWorldItemReferenceLists(collector);
            changed |= CollectWorldItemTemplateLists(collector);
            changed |= CollectPlayerLocalObjectReferenceLists(collector);
            return changed;
        }

        bool CollectWorldItemReferenceLists(PSAssetCollector collector)
        {
            if (!CollectEntries<WorldItemReferenceListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new WorldItemReferenceListAccess(GetOrAddComponent<WorldItemReferenceList>(collector));
            access.SetEntries(entries);
            return true;
        }

        bool CollectWorldItemTemplateLists(PSAssetCollector collector)
        {
            if (!CollectEntries<WorldItemTemplateListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new WorldItemTemplateListAccess(GetOrAddComponent<WorldItemTemplateList>(collector));
            access.SetEntries(entries);
            return true;
        }

        bool CollectPlayerLocalObjectReferenceLists(PSAssetCollector collector)
        {
            if (!CollectEntries<PlayerLocalObjectReferenceListAccessEntry>(out var entries))
            {
                return false;
            }
            using var access = new PlayerLocalObjectReferenceListAccess(GetOrAddComponent<PlayerLocalObjectReferenceList>(collector));
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
