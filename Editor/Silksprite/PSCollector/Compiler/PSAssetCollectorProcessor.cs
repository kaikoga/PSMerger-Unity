using System.Collections.Generic;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Silksprite.PSCore.Access;
using UnityEngine;
using PlayerLocalObjectReferenceListEntry = Silksprite.PSCore.Access.PlayerLocalObjectReferenceListEntry;

namespace Silksprite.PSCollector.Compiler
{
    public class PSAssetCollectorProcessor
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
            var mergedWirls = CollectSources<MergedWorldItemReferenceList>().ToArray();
            if (mergedWirls.SelectMany(mergedWirl => mergedWirl.WorldItemReferences).Any())
            {
                if (!collector.gameObject.TryGetComponent<WorldItemReferenceList>(out var wirl))
                {
                    wirl = collector.gameObject.AddComponent<WorldItemReferenceList>();
                }
                using var wirlAccess = new WorldItemReferenceListAccess(wirl);
                var entries = mergedWirls
                    .SelectMany(mergedWirl => mergedWirl.WorldItemReferences)
                    .Select(wir => new WorldItemReferenceListAccessEntry
                    {
                        id = wir.id,
                        item = wir.item
                    });
                wirlAccess.SetEntries(entries);
            }
            var mergedPlorls = CollectSources<MergedPlayerLocalObjectReferenceList>().ToArray();
            if (mergedPlorls.SelectMany(mergedPlorl => mergedPlorl.PlayerLocalObjectReferences).Any())
            {
                if (!collector.gameObject.TryGetComponent<PlayerLocalObjectReferenceList>(out var plorl))
                {
                    plorl = collector.gameObject.AddComponent<PlayerLocalObjectReferenceList>();
                }
                using var plorlAccess = new PlayerLocalObjectReferenceListAccess(plorl);
                var entries = mergedPlorls
                    .SelectMany(mergedPlorl => mergedPlorl.PlayerLocalObjectReferences)
                    .Select(plor => new PlayerLocalObjectReferenceListEntry
                    {
                        id = plor.id,
                        targetObject = plor.targetObject
                    });
                plorlAccess.SetEntries(entries);
            }
            return true;
        }
    }
}
