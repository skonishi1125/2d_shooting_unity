## Cosmo Phoot

<img width="600" alt="image" src="https://github.com/user-attachments/assets/11bee5c6-6d89-45b0-ae39-5e4a5d4e7db0" />

<img width="600" alt="image" src="https://github.com/user-attachments/assets/2c938d21-59fb-40b1-8bde-926617aad757" />

<img width="1872" height="932" alt="image" src="https://github.com/user-attachments/assets/ce719362-fc9c-4e4f-af86-e49a70e6a0e6" />

### ゲームについて
#### URL
https://unityroom.com/games/251130_cosmo_phoot

#### 操作方法
```
【操作説明】
左クリック : ショット
右クリック: 低速移動
Escape : ポーズ / ポーズ解除
WASD | ↑→←↓ : 移動

敵を10匹倒すと、アイテムがドロップします。
アイテムは時間経過で色々な効果に変化します。

Damage : 攻撃力が上がります。
Fire Rate: 射撃間隔が減少し、弾をたくさん打てるようになります。
Distance: 弾が遠くまで届くようになります。
```

#### ゲームの説明
上下移動・ショットを駆使して進む 2D 横スクロールシューティングです。

ステージ中に取得できるアイテムでプレイヤーを強化ができます。

ステージごとに異なる移動パターンを持つボスと戦う構成になっています。

### 開発環境等
* 製作時間：100時間ほど
* Unity Ver：6000.0.60f1
* ジャンル：2D シューティング
* 作業範囲：ゲームデザイン / プログラム / UIなど



### 作成の目的
以下の理解を目的としました。
* 基本的な当たり判定処理の実装方法
* GameManagerを使用した、ゲーム全体の状態管理方法
* SFXなどオーディオの割当て方法
* UIの基本的な実装方法
* 負荷軽減のためのObjectPoolパターンの仕組み
* ScriptableObjectを用いた敵出現パターンの管理方法


### 考慮した部分など
#### GameManager
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/GameManager.cs

* DontDestroyOnLoadでScene遷移し、共通して使えるようにする
* ステージ遷移処理、暗転演出、プレイヤーのステータス引継ぎ
* ポーズやクリアフラグ、ゲームオーバー等ゲーム全体の制御

#### ObjectPool
* 弾丸をInstantiate -> Destroyとせず、Poolから再利用するようにした
  * https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/ObjectPool/BulletPool.cs
  * https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/ObjectPool/PooledBullet.cs
  * https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Common/BulletBase.cs
  * https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Player/PlayerShooter.cs

#### 自機周り
https://github.com/skonishi1125/2d_shooting_unity/tree/main/Assets/Scripts/Player

* New Input Systemの利用
* 各ステータスの上昇処理
* GameManagerに上昇値を持たせ、シーン跨ぎのステータス引継ぎ対応
* デバッグ用に、GameManagerを介して各ステータスをInspector上で調整できるようにした

#### 敵スポーン
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/Stage/WaveData.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/Stage/StageData.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/Stage/StageController.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/Stage/EnemySpawner.cs

* スポナーの実装
* Wave単位でスポーンを区切り、WaveをまとめたものをStageとして運用
* 敵の出現パターンをScriptableObjectで定義

#### その他
* バージョン管理※アニメーション等の外部素材はignore済
* 操作説明画面にて、keyをNewInputSystemから取得して文字列が動的に変わるように
* 被弾時の無敵判定処理
* 低速移動時、自身の当たり判定が見えるように
* 敵の共通クラス管理
* DOTween等外部ライブラリの使用

### お借りした素材など
```
効果音
https://otologic.jp/free/license.html
https://umipla.com/
```

