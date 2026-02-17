using UnityEngine;
using UnityEngine.UI;

public class AutoRetroLevelBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsurePlayableScene()
    {
        if (FindObjectOfType<PlayerController2D>() != null)
        {
            return;
        }

        Bootstrap();
    }

    private static void Bootstrap()
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer < 0)
        {
            groundLayer = 0;
        }

        GameObject sessionObj = new GameObject("GameSession");
        sessionObj.AddComponent<GameSession>();

        Sprite boxSprite = CreateSolidSprite();

        GameObject player = CreateBoxObject("Player", new Vector2(-6f, -1f), new Vector2(0.8f, 1.2f), new Color(0.95f, 0.2f, 0.2f), boxSprite, 0, false);
        Rigidbody2D playerRb = player.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 3f;
        playerRb.freezeRotation = true;
        PlayerController2D playerController = player.AddComponent<PlayerController2D>();
        SalarymanVisual2D playerVisual = player.AddComponent<SalarymanVisual2D>();
        player.AddComponent<CodeShooter2D>();
        player.AddComponent<RetroPixelSnap2D>();

        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0f, -0.62f, 0f);
        playerController.ConfigureGroundCheck(groundCheck.transform, Physics2D.DefaultRaycastLayers);
        playerController.ConfigureVisualRoot(playerVisual.VisualRoot);

        CreateGround(groundLayer, boxSprite);
        CreatePlatforms(groundLayer, boxSprite);
        CreateEnemies(groundLayer, boxSprite);
        CreateCodePowerUps(boxSprite);
        CreateCoins(boxSprite);
        CreateGoal(boxSprite);
        CreateKillZone();
        SetupCamera(player.transform);
        CreateHud();
    }

    private static void CreateGround(int groundLayer, Sprite sprite)
    {
        CreateBoxObject("Ground", new Vector2(0f, -3.5f), new Vector2(40f, 2f), new Color(0.2f, 0.65f, 0.25f), sprite, groundLayer, false);
    }

    private static void CreatePlatforms(int groundLayer, Sprite sprite)
    {
        CreateBoxObject("PlatformA", new Vector2(-1.5f, -0.5f), new Vector2(4f, 0.7f), new Color(0.35f, 0.5f, 0.9f), sprite, groundLayer, false);
        CreateBoxObject("PlatformB", new Vector2(4f, 1.2f), new Vector2(4f, 0.7f), new Color(0.35f, 0.5f, 0.9f), sprite, groundLayer, false);
    }

    private static void CreateEnemies(int groundLayer, Sprite sprite)
    {
        CreateEnemy(new Vector2(-0.2f, -2.3f), groundLayer, sprite);
        CreateEnemy(new Vector2(5.2f, -2.3f), groundLayer, sprite);
    }

    private static void CreateEnemy(Vector2 position, int groundLayer, Sprite sprite)
    {
        GameObject enemy = CreateBoxObject("Enemy", position, new Vector2(0.9f, 0.9f), new Color(0.5f, 0.25f, 0.1f), sprite, 0, false);
        Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2.5f;
        rb.freezeRotation = true;

        EnemyPatrol2D patrol = enemy.AddComponent<EnemyPatrol2D>();
        BugEnemyVisual2D enemyVisual = enemy.AddComponent<BugEnemyVisual2D>();
        enemy.AddComponent<RetroPixelSnap2D>();

        GameObject wallCheck = new GameObject("WallCheck");
        wallCheck.transform.SetParent(enemy.transform);
        wallCheck.transform.localPosition = new Vector3(-0.48f, 0f, 0f);

        GameObject edgeCheck = new GameObject("EdgeCheck");
        edgeCheck.transform.SetParent(enemy.transform);
        edgeCheck.transform.localPosition = new Vector3(-0.42f, -0.45f, 0f);

        int mask = groundLayer == 0 ? Physics2D.DefaultRaycastLayers : LayerMask.GetMask("Ground");
        patrol.ConfigureChecks(wallCheck.transform, edgeCheck.transform, mask);
        patrol.ConfigureVisualRoot(enemyVisual.VisualRoot);
    }

    private static void CreateCodePowerUps(Sprite sprite)
    {
        CreateCodePowerUp(new Vector2(-4.8f, -2.1f), "=", sprite);
        CreateCodePowerUp(new Vector2(-3.4f, -2.1f), "=", sprite);
        CreateCodePowerUp(new Vector2(-2.0f, -2.1f), "sum(", sprite);
        CreateCodePowerUp(new Vector2(-0.6f, -2.1f), "arr", sprite);
        CreateCodePowerUp(new Vector2(0.8f, -2.1f), ")", sprite);

        CreateCodePowerUp(new Vector2(2.2f, -0.1f), "+", sprite);
        CreateCodePowerUp(new Vector2(3.6f, 0.3f), "max(", sprite);
        CreateCodePowerUp(new Vector2(4.9f, 0.9f), "x", sprite);
        CreateCodePowerUp(new Vector2(6.0f, 1.3f), ",", sprite);
        CreateCodePowerUp(new Vector2(7.0f, 1.6f), "y", sprite);
        CreateCodePowerUp(new Vector2(8.0f, 1.9f), ")", sprite);
    }

    private static void CreateCodePowerUp(Vector2 position, string token, Sprite sprite)
    {
        GameObject item = new GameObject("CodePowerUp");
        item.transform.position = position;
        item.transform.localScale = new Vector3(0.52f, 0.52f, 1f);

        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = new Color(0.2f, 0.95f, 0.55f);
        renderer.sortingOrder = 30;

        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        GameObject label = new GameObject("Label");
        label.transform.SetParent(item.transform);
        label.transform.localPosition = new Vector3(0f, 0f, -0.01f);

        TextMesh textMesh = label.AddComponent<TextMesh>();
        textMesh.text = token;
        textMesh.characterSize = 0.22f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = new Color(0.03f, 0.15f, 0.08f);
        textMesh.fontSize = 36;

        CodePowerUp2D powerUp = item.AddComponent<CodePowerUp2D>();
        powerUp.SetToken(token);
    }

    private static void CreateCoins(Sprite sprite)
    {
        CreateCoin(new Vector2(-1.5f, 0.6f), sprite);
        CreateCoin(new Vector2(-0.5f, 0.6f), sprite);
        CreateCoin(new Vector2(3.2f, 2.2f), sprite);
        CreateCoin(new Vector2(4.2f, 2.2f), sprite);
    }

    private static void CreateCoin(Vector2 position, Sprite sprite)
    {
        GameObject coin = new GameObject("Coin");
        coin.transform.position = position;
        SpriteRenderer renderer = coin.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = new Color(1f, 0.85f, 0.1f);

        CircleCollider2D circle = coin.AddComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.radius = 0.35f;

        coin.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        coin.AddComponent<CoinPickup2D>();
    }

    private static void CreateGoal(Sprite sprite)
    {
        GameObject goal = CreateBoxObject("Goal", new Vector2(9.5f, -1.2f), new Vector2(0.7f, 4.2f), new Color(0.95f, 0.95f, 0.95f), sprite, 0, true);
        goal.AddComponent<GoalPole2D>();
    }

    private static void CreateKillZone()
    {
        GameObject killZone = new GameObject("KillZone");
        killZone.transform.position = new Vector2(0f, -6.5f);

        BoxCollider2D collider = killZone.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(100f, 1f);
        collider.isTrigger = true;

        killZone.AddComponent<KillZone2D>();
    }

    private static void SetupCamera(Transform player)
    {
        Camera main = Camera.main;
        if (main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            main = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
            main.orthographic = true;
            main.orthographicSize = 5f;
        }

        CameraFollow2D follow = main.GetComponent<CameraFollow2D>();
        if (follow == null)
        {
            follow = main.gameObject.AddComponent<CameraFollow2D>();
        }

        RetroPixelSnap2D snap = main.GetComponent<RetroPixelSnap2D>();
        if (snap == null)
        {
            snap = main.gameObject.AddComponent<RetroPixelSnap2D>();
        }

        follow.SetTarget(player);
    }

    private static void CreateHud()
    {
        GameObject canvasObj = new GameObject("HUD");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Text coinText = CreateHudText(canvasObj.transform, "CoinText", new Vector2(90f, -24f), font);
        Text scoreText = CreateHudText(canvasObj.transform, "ScoreText", new Vector2(290f, -24f), font);
        Text lifeText = CreateHudText(canvasObj.transform, "LifeText", new Vector2(500f, -24f), font);
        Text codeText = CreateHudText(canvasObj.transform, "CodeText", new Vector2(90f, -66f), font);

        HudText2D hud = canvasObj.AddComponent<HudText2D>();
        hud.SetTexts(coinText, scoreText, lifeText);
        hud.SetCodeText(codeText);
    }

    private static Text CreateHudText(Transform parent, string name, Vector2 anchoredPos, Font font)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.sizeDelta = new Vector2(200f, 40f);
        rect.anchoredPosition = anchoredPos;

        Text text = textObj.AddComponent<Text>();
        text.font = font;
        text.fontSize = 26;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        text.text = "...";

        return text;
    }

    private static GameObject CreateBoxObject(string name, Vector2 position, Vector2 size, Color color, Sprite sprite, int layer, bool trigger)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.layer = layer;

        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;

        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;
        collider.isTrigger = trigger;

        obj.transform.localScale = new Vector3(size.x, size.y, 1f);
        return obj;
    }

    private static Sprite CreateSolidSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }

}