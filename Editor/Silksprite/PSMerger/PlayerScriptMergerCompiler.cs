using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClusterVR.CreatorKit.Item.Implements;
using Google.Protobuf.WellKnownTypes;
using UnityEditor;

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
                playerScriptAccess.sourceCode = BuildPlayerScript(playerScriptMerger);
                changed |= playerScriptAccess.hasModifiedProperties;
            }

            return changed;
        }

        static string BuildPlayerScript(PlayerScriptMerger playerScriptMerger)
        {
            return PlayerScriptPreamble + string.Join("\n", playerScriptMerger.PlayerScripts.Select(ps => $@"
(_ => {{
{ps.text}
}})(__());
"));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        class ScriptableItemAccess : IDisposable
        {
            readonly SerializedObject _serializedObject;
            
            public bool hasModifiedProperties => _serializedObject.hasModifiedProperties;

            public JavaScriptAsset sourceCodeAsset
            {
                set
                {
                    using var prop = _serializedObject.FindProperty("sourceCodeAsset");
                    if (prop.objectReferenceValue != value) prop.objectReferenceValue = value;
                }
            }

            public string sourceCode
            {
                set
                {
                    using var prop = _serializedObject.FindProperty("sourceCode");
                    if (prop.stringValue != value) prop.stringValue = value;
                }
            }

            public ScriptableItemAccess(ScriptableItem scriptableItem) => _serializedObject = new SerializedObject(scriptableItem);

            void IDisposable.Dispose() => _serializedObject.ApplyModifiedProperties();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        class PlayerScriptAccess : IDisposable
        {
            readonly SerializedObject _serializedObject;

            public bool hasModifiedProperties => _serializedObject.hasModifiedProperties;

            public JavaScriptAsset sourceCodeAsset
            {
                set
                {
                    using var prop = _serializedObject.FindProperty("sourceCodeAsset");
                    if (prop.objectReferenceValue != value) prop.objectReferenceValue = value;
                }
            }

            public string sourceCode
            {
                set
                {
                    using var prop = _serializedObject.FindProperty("sourceCode");
                    if (prop.stringValue != value) prop.stringValue = value;
                }
            }

            public PlayerScriptAccess(PlayerScript playerScript) => _serializedObject = new SerializedObject(playerScript);

            void IDisposable.Dispose() => _serializedObject.ApplyModifiedProperties();
        }
    }
}
