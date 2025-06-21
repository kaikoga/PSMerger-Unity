using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using UnityEngine;

namespace Silksprite.PSCollector.Compiler
{
    public class PSAssetCollectorCompiler
    {
        IEnumerable<T> CollectSources<T>()
            where T : Component
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            return rootObjects.SelectMany(o => o.GetComponentsInChildren<T>(true));
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
            var entries = CollectSources<MergedWorldItemReferenceList>()
                .SelectMany(entry => entry.WorldItemReferences)
                .ToArray();
            if (!entries.Any())
            {
                return false;
            }
            using var access = new WorldItemReferenceListAccess(GetOrAddComponent<WorldItemReferenceList>(collector));
            access.SetEntries(entries);
            return true;
        }

        bool CollectWorldItemTemplateLists(PSAssetCollector collector)
        {
            var entries = CollectSources<MergedWorldItemTemplateList>()
                .SelectMany(entry => entry.WorldItemTemplates)
                .ToArray();
            if (!entries.Any())
            {
                return false;
            }
            using var access = new WorldItemTemplateListAccess(GetOrAddComponent<WorldItemTemplateList>(collector));
            access.SetEntries(entries);
            return true;
        }

        bool CollectPlayerLocalObjectReferenceLists(PSAssetCollector collector)
        {
            var entries = CollectSources<MergedPlayerLocalObjectReferenceList>()
                .SelectMany(entry => entry.PlayerLocalObjectReferences)
                .ToArray();
            if (!entries.Any())
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
