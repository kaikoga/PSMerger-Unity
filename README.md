## PSMerger

[PlayerScript](https://docs.cluster.mu/script/interfaces/PlayerScript.html) and [ItemScript](https://docs.cluster.mu/script/interfaces/ClusterScript.html) Merger

<img src="screenshot.png" width="300" />

## Merges ItemScript

- ClusterScript.onStart()
- ClusterScript.onUpdate()
- ClusterScript.onInteract()
- ClusterScript.onTextInput()
- ClusterScript.onExternalCallEnd()
- ...

## Merges PlayerScript

- PlayerScript.onFrame()
- PlayerScript.onReceive()
- PlayerScript.onButton()
- OscHandle.onReceive()

## Caveats

- Callbacks for responses of API calls by other contexts are called in all contexts.
  You should check `meta` whether you want to process the callback result.  
- [ClusterScript.onPurchaseUpdated()](https://docs.cluster.mu/script/interfaces/ClusterScript.html#onPurchaseUpdated) receives all subscribed purchases (in all contexts) by [ClusterScript.subscribePurchase()](https://docs.cluster.mu/script/interfaces/ClusterScript.html#subscribePurchase). 
- [UnityComponent.onClick()](https://docs.cluster.mu/script/interfaces/UnityComponent.html#onClick) can be used, but not coexist. (only the last registration will take effect.)

## Install

```json5
// Packages/manifest.json
{
  "dependencies": {
    // ...
    "net.kaikoga.psmerger": "https://github.com/kaikoga/PSMerger-Unity.git",
    // ...
  }
}
```
