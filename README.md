## Cosmo Phoot
https://unityroom.com/games/251130_cosmo_phoot

<img width="600" alt="image" src="https://github.com/user-attachments/assets/11bee5c6-6d89-45b0-ae39-5e4a5d4e7db0" />

<img width="600" alt="image" src="https://github.com/user-attachments/assets/2c938d21-59fb-40b1-8bde-926617aad757" />

<img width="1872" height="932" alt="image" src="https://github.com/user-attachments/assets/ce719362-fc9c-4e4f-af86-e49a70e6a0e6" />

### 作業環境など
開発期間：2週間
Unity Vesion：6000.0.60f1
ジャンル：2D シューティング
担当範囲：ゲームデザイン / プログラム / UIなど

### ゲームの説明
上下移動・ショットを駆使して進む 2D 横スクロールシューティングです。

ステージ中に取得できるアイテムでプレイヤーを強化ができます。

ステージごとに異なる移動パターンを持つボスと戦う構成になっています。


### 考慮したパターンや設計
#### GameManager
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/GameManager.cs

以下の設計を考慮しています。
* DontDestroyOnLoadでScene遷移し、共通して使えるようにする
* ステージ遷移処理、暗転演出、プレイヤーのステータス引継ぎ
* ポーズやクリアフラグ、ゲームオーバー等ゲーム全体の制御

#### ObjectPool
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/ObjectPool/BulletPool.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/ObjectPool/PooledBullet.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Common/BulletBase.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Player/PlayerShooter.cs

以下の設計を考慮しています。
* 弾丸をInstantiate -> Destroyとせず、Poolから再利用するようにした

#### ステータス管理
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/GameManager.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Player/PlayerStatus.cs

以下の設計を考慮しています。
* ステータスの上昇処理
* 各ステータスのレベル管理
* デバッグ用プリセットが適用できるようにする
* GameManagerに上昇値を持たせ、シーン跨ぎの　ステータス引継ぎ対応

#### 敵スポーン
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/WaveData.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/StageData.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/StageController.cs
https://github.com/skonishi1125/2d_shooting_unity/blob/main/Assets/Scripts/Game/EnemySpawner.cs

以下の設計を考慮しています。
* スポナーの実装
* Wave単位でスポーンを区切り、WaveをまとめたものをStageとして運用
* 敵の出現パターンをScriptableObjectで定義

#### その他
* バージョン管理
* 被弾時の無敵判定処理
* デバッグ機能
* 低速移動時、自身の当たり判定が見えるように
* 敵の共通クラス管理
* 外部ライブラリ(DOTween)の使用

