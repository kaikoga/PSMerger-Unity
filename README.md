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

- Attach a `PlayerScript Merger` or `ItemScript Merger` component to ScriptableItems to merge scripts in place.
  - For `PlayerScript Merger`, ItemScript of the item is automatically set up to apply the merged PlayerScript to all players in the space.
- Create a `ClusterScriptAssetMerger` via `Assets` -> `Create` -> `Silksprite` -> `PSMerger` -> `ClusterScriptAssetMerger` to merge scripts into a JavaScriptAsset.
  - Experimental SourceMap support is available on PSMerger 2.0.0 or over.

### Advanced Settings

- `Inline JavaScript`
  - If unset slots are present in `PlayerScript Merger` or `ItemScript Merger`, the content of `Inline JavaScript` is merged at the place of the unset slot.
  - If multiple unset slots are present, applied to every unset slots. (Code will be duplicated.)

In most cases, you will not need to edit the settings below.

- `Merged Script`
  - Change outputs of `PlayerScript Merger` or `ItemScript Merger` from the component to JavaScript asset.
  - Would conflict if the same JavaScript asset is specified to multiple `Merged Script`.
- `UseGlobalPlayerScriptSetter`
  - If on, and `PlayerScript Merger` does not also have `ItemScript Merger`, a script to do `$.setPlayerScript()` players in the space is added.
  - If on, and `PlayerScript Merger` also has `ItemScript Merger`, has no effect. (Output of `ItemScript Merger` takes priority.)
  - If off, nothing happens.
  - Defaults to on.
- `Generate Sourcemap`
  - If on, generates Sourcemap.
  - If sourcemap is generated, code jumping in [ClusterScript Log Console Window 2](https://kaikoga.booth.pm/items/7084684) becomes smarter, for example.
  - If used by `PlayerScript Merger` or `ItemScript Merger`, `Merged Script` must be also specified.
  - Defaults to off.
- `DetectCallbackSupport`
  - If on, code to support callbacks only present in the source code is attempted to be generated.
  - If off, code to support all callbacks is generated. (Code size may increase.)
  - Defaults to on.

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

# PSMerger CSExEx

https://github.com/kaikoga/PSMergerCSExEx-Unity