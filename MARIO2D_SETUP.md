# Unity 2D マリオ風アクション セットアップ手順

このプロジェクトには、スーパーファミコン時代の横スクロールアクションの基礎挙動を実装済みです。

## まず遊ぶ（最短）

このプロジェクトは、シーンに何も配置されていなくても再生時に自動でプレイ用ステージを生成します。

1. Unity 6（`6000.3.8f1`）でプロジェクトを開く
2. 任意のシーンを開く（空シーンでも可）
3. Play ボタンを押す

操作:

- 左右移動: A/D または ←/→
- ジャンプ: Space
- コード射撃: J（または Left Ctrl）

## Unityでの実行方法（調査結果）

### Unity Hub から開く

1. Unity Hub を起動
2. Add > Add project from disk でこのフォルダを選択
3. Editor Version は `6000.3.8f1` を選択（未導入なら Install）
4. Open でプロジェクトを開く

### エディタで実行

1. `Scenes` 内の任意シーンを開く
2. 上部の ▶ (Play) を押す
3. 停止は ■ (Stop)

### ビルドして実行（Windows）

1. File > Build Settings
2. `Add Open Scenes` でシーン追加
3. Platform: Windows を選び `Switch Platform`
4. `Build And Run` を押して出力先を選択

## 追加済みスクリプト

- Assets/Scripts/Player/PlayerController2D.cs
- Assets/Scripts/Enemy/EnemyPatrol2D.cs
- Assets/Scripts/Items/CoinPickup2D.cs
- Assets/Scripts/Level/GoalPole2D.cs
- Assets/Scripts/Hazards/KillZone2D.cs
- Assets/Scripts/Core/GameSession.cs
- Assets/Scripts/Core/HudText2D.cs
- Assets/Scripts/Camera/CameraFollow2D.cs
- Assets/Scripts/Bootstrap/AutoRetroLevelBootstrap.cs
- Assets/Scripts/Visuals/SalarymanVisual2D.cs
- Assets/Scripts/Visuals/BugEnemyVisual2D.cs
- Assets/Scripts/Visuals/RetroPixelSnap2D.cs
- Assets/Scripts/Combat/CodeShooter2D.cs
- Assets/Scripts/Combat/CodeProjectile2D.cs
- Assets/Scripts/Items/CodePowerUp2D.cs

## 1. レイヤー作成

Unity の Layer に以下を追加します。

- Ground
- Enemy
- Player
- Item
- Goal
- Hazard

## 2. プレイヤー設定

1. 空オブジェクト `Player` を作成
2. 以下を追加
   - Rigidbody2D（Gravity Scale: 3 前後）
   - CapsuleCollider2D
   - PlayerController2D
  - SalarymanVisual2D
  - CodeShooter2D
3. `Player` の子に `GroundCheck` 空オブジェクトを作成（足元）
4. PlayerController2D の `Ground Check` に `GroundCheck` を割り当て
5. PlayerController2D の `Ground Layer` は Ground を指定
6. 見た目用に SpriteRenderer を子に追加

## 3. 地形設定

1. タイルマップまたは Sprite で床を配置
2. Collider2D（TilemapCollider2D / BoxCollider2D）を付与
3. Layer を Ground に設定

## 4. 敵設定

1. `Enemy` オブジェクト作成
2. 追加コンポーネント
   - Rigidbody2D（Body Type: Dynamic）
   - BoxCollider2D
   - EnemyPatrol2D
  - BugEnemyVisual2D
3. 子に `WallCheck` と `EdgeCheck` を作成
4. EnemyPatrol2D の `Wall Check` と `Edge Check` に割り当て
5. `Ground Layer` を Ground に設定

## 5. コイン設定

1. `Coin` オブジェクト作成
2. CircleCollider2D を Is Trigger = true
3. CoinPickup2D を追加

## 6. ゴール設定

1. `Goal` オブジェクト作成
2. BoxCollider2D を Is Trigger = true
3. GoalPole2D を追加

## 6.5 コード強化アイテム設定

1. `CodePowerUp` オブジェクト作成
2. BoxCollider2D を Is Trigger = true
3. CodePowerUp2D を追加

## 7. 落下死ゾーン

1. 画面下に横長オブジェクト `KillZone` を作成
2. BoxCollider2D を Is Trigger = true
3. KillZone2D を追加

## 8. ゲーム管理とカメラ

1. 空オブジェクト `GameSession` を作成して GameSession を追加
2. Main Camera に CameraFollow2D を追加
3. CameraFollow2D の target に Player を割り当て

## 9. HUD 表示（任意）

1. Canvas を作成
2. Text (Legacy) を3つ作成（COIN/SCORE/LIFE）
3. Canvas か空オブジェクトに HudText2D を追加
4. 3つの Text を HudText2D に割り当て

## 10. Build Settings

- File > Build Settings で現在シーンを追加
- 複数シーンを追加すれば、ゴール時に次シーンへ遷移

## 操作

- 左右移動: A/D または ←/→
- ジャンプ: Space
- コード射撃: J（または Left Ctrl）

## 調整ポイント

- PlayerController2D
  - moveSpeed
  - jumpForce
  - coyoteTime
- EnemyPatrol2D
  - moveSpeed
- GameSession
  - startLives
  - respawnDelay
- CodeShooter2D
  - shootInterval
  - codeLevels（初期 `i`、強化で長いコードへ）
