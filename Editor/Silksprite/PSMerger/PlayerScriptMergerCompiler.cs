using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using PlasticGui;
using Silksprite.PSMerger.Access;

namespace Silksprite.PSMerger
{
    public static class PlayerScriptMergerCompiler
    {
        const string ItemScript = @"
$.onStart(() => {
    $.state.players = [];
});
$.onUpdate(_ => {
    let lastPlayers = $.state.players;
    let players = $.getPlayersNear($.getPosition(), Infinity);
    for (let player of players) {
        if (lastPlayers.every(p => p.id !== player.id)) {
            $.setPlayerScript(player);
        }
    }
    $.state.players = players;
});
";

        const string PlayerScriptPreamble = @"
const __ = (() => {
    const onFrame = new Map();
    const onReceive = new Map();
    const onOscReceive = new Map();
    _.onFrame(deltaTime => {
        for (const f of onFrame.values()) f(deltaTime);
    });
    _.onReceive((messageType, arg, sender) => {
        for (const f of onReceive.values()) f(messageType, arg, sender);
    });
    _.oscHandle.onReceive((messages) => {
        for (const f of onOscReceive.values()) f(messages);
    });
    function createProxy(obj, base) {
        return new Proxy(obj, {
            get(target, prop, receiver) {
                let thisValue = target; 
                let value = target[prop];
                if (value === undefined) {
                    thisValue = base;
                    value = base[prop];
                }
                return (value instanceof Function) ? value.bind(thisValue) : value;
            }
        });
    }
    return () => {
        const _oscHandle = createProxy({
            onOscReceive(callback) { onOscReceive.set(this, callback) },
        }, _.oscHandle);
        return createProxy({
            onFrame(callback) { onFrame.set(this, callback) },
            onReceive(callback) { onReceive.set(this, callback) },
            get oscHandle() { return _oscHandle },
        }, _);
    }
})();
";

        public static bool Compile(PlayerScriptMerger playerScriptMerger)
        {
            var changed = false;
            using (var scriptableItemAccess = new ScriptableItemAccess(playerScriptMerger.GetComponent<ScriptableItem>()))
            {
                scriptableItemAccess.sourceCodeAsset = null;
                scriptableItemAccess.sourceCode = ItemScript;
                changed |= scriptableItemAccess.hasModifiedProperties;
            }

            using (var playerScriptAccess = new PlayerScriptAccess(playerScriptMerger.GetComponent<PlayerScript>()))
            {
                playerScriptAccess.sourceCodeAsset = null;
                playerScriptAccess.sourceCode = BuildPlayerScript(playerScriptMerger.PlayerScripts);
                changed |= playerScriptAccess.hasModifiedProperties;
            }

            return changed;
        }

        public static bool Compile(PlayerScriptAssetMerger playerScriptAssetMerger)
        {
            using var javaScriptAssetAccess = new JavaScriptAssetAccess(playerScriptAssetMerger.MergedPlayerScript);
            javaScriptAssetAccess.text = BuildPlayerScript(playerScriptAssetMerger.PlayerScripts);
            return javaScriptAssetAccess.hasModifiedProperties;
        }

        static string BuildPlayerScript(JavaScriptAsset[] playerScripts)
        {
            return BuildPlayerScript(playerScripts.Select(ps => new[] { ps }).ToArray());
        }

        static string BuildPlayerScript(JavaScriptAsset[][] playerScripts)
        {
            return PlayerScriptPreamble + string.Join("\n", playerScripts.Select(context => $@"
(_ => {{
{string.Join("\n", context.Select(ps => ps != null ? ps.text : null))}
}})(__());
"));
        }

    }
}
