using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using UnityEngine;
using PlayerLocalObjectReferenceListEntry = Silksprite.PSCore.Access.PlayerLocalObjectReferenceListEntry;

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
            var mergedWirs = CollectSources<MergedWorldItemReferenceList>()
                .SelectMany(mergedWirl => mergedWirl.WorldItemReferences)
                .ToArray();
            if (!mergedWirs.Any())
            {
                return false;
            }
            using var wirlAccess = new WorldItemReferenceListAccess(GetOrAddComponent<WorldItemReferenceList>(collector));
            var entries = mergedWirs
                .Select(wir => new WorldItemReferenceListAccessEntry
                {
                    id = wir.id,
                    item = wir.item
                });
            wirlAccess.SetEntries(entries);
            return true;
        }

        bool CollectWorldItemTemplateLists(PSAssetCollector collector)
        {
            var mergedWits = CollectSources<MergedWorldItemTemplateList>()
                .SelectMany(mergedWit => mergedWit.WorldItemTemplates)
                .ToArray();
            if (!mergedWits.Any())
            {
                return false;
            }
            using var witlAccess = new WorldItemTemplateListAccess(GetOrAddComponent<WorldItemTemplateList>(collector));
            var entries = mergedWits
                .Select(wit => new WorldItemTemplateListAccessEntry
                {
                    id = wit.id,
                    worldItemTemplate = wit.worldItemTemplate
                });
            witlAccess.SetEntries(entries);
            return true;
        }

        bool CollectPlayerLocalObjectReferenceLists(PSAssetCollector collector)
        {
            var mergedPlors = CollectSources<MergedPlayerLocalObjectReferenceList>()
                .SelectMany(mergedPlorl => mergedPlorl.PlayerLocalObjectReferences)
                .ToArray();
            if (!mergedPlors.Any())
            {
                return false;
            }
            using var plorlAccess = new PlayerLocalObjectReferenceListAccess(GetOrAddComponent<PlayerLocalObjectReferenceList>(collector));
            var entries = mergedPlors
                .Select(plor => new PlayerLocalObjectReferenceListEntry
                {
                    id = plor.id,
                    targetObject = plor.targetObject
                });
            plorlAccess.SetEntries(entries);
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
