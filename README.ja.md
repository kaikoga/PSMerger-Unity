# PSMerger

[cluster](https://cluster.mu/)の[PlayerScript](https://docs.cluster.mu/script/interfaces/PlayerScript.html)や[ItemScript](https://docs.cluster.mu/script/interfaces/ClusterScript.html)をマージします。

<img src="screenshot.png" width="300" />

## これは何ですか？

- 複数のPlayerScriptを同時に動かしたい時に使います
- ItemScriptやPlayerScriptのソースコードを結合したい時に使います

### ItemScript

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

### PlayerScript

- `onButton()`
- `onFrame()`
- `onReceive()`
- `OscHandle.onReceive()`

## インストール方法（UPM git dependency）

```json
{
  "dependencies": {
    "net.kaikoga.psmerger": "https://github.com/kaikoga/PSMerger-Unity.git"
  }
}
```

## 使用方法

### ScriptableItem にマージ

ScriptableItem に `PlayerScript Merger` または `ItemScript Merger` コンポーネントを追加すると、その場でマージされます。

`PlayerScript Merger` を使う場合は、マージされたPlayerScriptがスペース内のすべてのプレイヤーに適用されるItemScriptが自動で設定されます。

### JavaScriptAsset にマージ

`Assets` -> `Create` -> `Silksprite` -> `PSMerger` -> `ClusterScriptAssetMerger`

を利用すると、マージされたスクリプトを JavaScript アセットに出力できます。
git 管理されたプロジェクトでプレハブの差分を減らしたい時に便利です。

PSMerger 2.0.0 以上で JavaScript アセットにマージした場合、 SourceMap を試験的に出力できます。

### 上級者向け設定

- `Inline JavaScript`
  - `PlayerScript Merger` や `ItemScript Merger` のスクリプトに未設定のスロットがある場合、 `Inline JavaScript` に入力した内容が未設定のスロットの場所にマージされます。
  - 複数の未設定のスロットがある場合、全てのスロットに適用されます。（コードが重複します。）

ほとんどの場合、以下の設定を編集する必要はありません。

- `Merged Script`
  - `PlayerScript Merger` や `ItemScript Merger` の出力先をコンポーネントではなく JavaScript アセットにできます。
  - 同じ JavaScript アセットを複数の場所に設定すると競合するので気をつけてください。
- `UseGlobalPlayerScriptSetter`
  - オン、かつ `PlayerScript Merger` に `ItemScript Merger` がついていない場合、スペースのプレイヤーに $.setPlayerScript() を行うスクリプトを追加します。
  - オン、かつ `PlayerScript Merger` に `ItemScript Merger` がついている場合、無効です。（ `ItemScript Merger` の出力が優先されます。）
  - オフの場合、何もしません。
  - デフォルトはオンです。
- `Generate Sourcemap`
  - オンの場合、ソースマップを生成します。
  - ソースマップを生成すると、例えば [ClusterScript Log Console Window 2](https://kaikoga.booth.pm/items/7084684) のコードを開く機能が賢くなります。
  - `PlayerScript Merger` や `ItemScript Merger` で利用する場合、 `Merged Script` の出力先の設定も必要です。
  - デフォルトはオフです。
- `DetectCallbackSupport`
  - オンの場合、ソースコード中に存在するコールバックに対応するコードのみを生成しようと試みます。
  - オフの場合、全てのコールバックに対応するコードを生成します。（コードサイズが増加します。）
  - デフォルトはオンです。

## 注意事項

PSMergerを使用する場合、一部のClusterScriptの挙動が異なります：

- `ClusterScript.onReceive()`は常に `{ item: true, player: true }` が設定されたものとして動作します。
- `ClusterScript.onPurchaseUpdated()` は他のスクリプトが `subscribePurchase()` した購入情報も受信します。
- `UnityComponent.onClick()` は共存できません。後勝ちになります。
- 他のスクリプト由来のコールバックも呼ばれるので、区別するために `meta` を活用してください。

# PSCollector

`Component` -> `Silksprite` -> `PSCollector` -> `PS Asset Collector`

がついたアイテムは、シーン内の `Merged Something List` の内容を収集して Creator Kit の `Something List` コンポーネントに出力します。
PSMerger でマージした PlayerScript の依存アイテムやアセットを設定するのに便利です。

注意: `Something List` コンポーネントの中身は上書きされます。

# PSMerger CSExEx

https://github.com/kaikoga/PSMergerCSExEx-Unity

ClusterScriptExtensions ([BOOTH](https://booth.pm/ja/items/6089933)) と一緒にインストールすることで、 PSMerger でマージするスクリプトに ClusterScriptExtensions のコメントを書けるようになります。

`PlayerScript Merger Extension` と `ItemScript Merger Extension` コンポーネントを追加することで、プロパティの編集がインスペクタから行えるようになります。
