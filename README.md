# PSMerger

A Unity package that merges [PlayerScript](https://docs.cluster.mu/script/interfaces/PlayerScript.html) and [ItemScript](https://docs.cluster.mu/script/interfaces/ClusterScript.html) callbacks from multiple input scripts in Cluster.

<img src="screenshot.png" width="300" />

## What is this?

PSMerger allows you to combine multiple ItemScript or PlayerScript source files into a single script, enabling you to use multiple script functionalities simultaneously in Cluster.

### ItemScript Callbacks
- `onCollide()`
- `onCommentReceived()`
- `onExternalCallEnd()`
- `onGetOwnProducts()`
- `onGiftSent()`
- `onGrab()`
- `onInteract()`
- `onPhysicsUpdate()`
- `onPurchaseUpdated()`
- `onReceive()`
- `onRequestGrantProductResult()`
- `onRequestPurchaseStatus()`
- `onRide()`
- `onStart()`
- `onSteer()`
- `onSteerAdditionalAxis()`
- `onTextInput()`
- `onUpdate()`
- `onUse()`

### PlayerScript Callbacks
- `onButton()`
- `onFrame()`
- `onReceive()`
- `OscHandle.onReceive()`

## Installation

Add the following to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "net.kaikoga.psmerger": "https://github.com/kaikoga/PSMerger-Unity.git"
  }
}
```

## Usage

Two modes are supported:

- Attach a `PlayerScriptMerger` or `ItemScriptMerger` component to ScriptableItems to merge scripts in place.
  - For `PlayerScriptMerger`, ItemScript of the item is automatically set up to apply the merged PlayerScript to all players in the space.
- Create a `ClusterScriptAssetMerger` via `Assets` -> `Create` -> `Silksprite` -> `PSMerger` -> `ClusterScriptAssetMerger` to merge scripts into a JavaScriptAsset.
  - Experimental SourceMap support is available on PSMerger 2.0.0 or over.

## Important Notes

When using PSMerger, there are some behavioral differences from standard Cluster scripting:

- `ClusterScript.onReceive()` receives messages from both ItemHandle and PlayerHandle, regardless of the second parameter
- `ClusterScript.onPurchaseUpdated()` receives all subscribed purchases from all input scripts
- `UnityComponent.onClick()` registrations are not merged - only the last registration takes effect
- API call responses are broadcast to all input scripts - use the `meta` parameter to filter responses

# PSCollector

`Component` -> `Silksprite` -> `PSCollector` -> `PS Asset Collector`

Collects contents of `Merged Something List` in the scene and populates `Something List` components of Creator Kit.
Useful for configuring the Item and Asset dependencies of the PlayerScript merged with PSMerger. 

Warning: The contents of `Something List` components are overwritten.
