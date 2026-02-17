using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CodeProjectile2D : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2.2f;

    private Vector2 moveDirection = Vector2.right;
    private int remainingEnemyHits;

    public void Initialize(Vector2 direction, string codeText, int powerLevel)
    {
        moveDirection = direction.normalized;
        string normalizedCode = string.IsNullOrWhiteSpace(codeText) ? "i" : codeText;
        remainingEnemyHits = Mathf.Max(0, powerLevel);

        speed += powerLevel * 1.4f;
        CreateVisual(normalizedCode, powerLevel);
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyPatrol2D enemy = other.GetComponent<EnemyPatrol2D>();
        if (enemy != null)
        {
            enemy.DefeatByCode();
            if (remainingEnemyHits > 0)
            {
                remainingEnemyHits--;
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        if (other.GetComponent<PlayerController2D>() != null)
        {
            return;
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    private void CreateVisual(string code, int powerLevel)
    {
        Sprite sprite = CreateSolidSprite();
        int length = Mathf.Max(1, code.Length);
        float width = Mathf.Min(2.8f, 0.32f + (length * 0.13f));
        float height = 0.28f + Mathf.Min(0.12f, powerLevel * 0.02f);

        GameObject body = new GameObject("Body");
        body.transform.SetParent(transform);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(width, height, 1f);

        SpriteRenderer bodyRenderer = body.AddComponent<SpriteRenderer>();
        bodyRenderer.sprite = sprite;
        bodyRenderer.color = new Color(0.06f, 0.18f, 0.12f);
        bodyRenderer.sortingOrder = 40;

        GameObject border = new GameObject("Border");
        border.transform.SetParent(transform);
        border.transform.localPosition = Vector3.zero;
        border.transform.localScale = new Vector3(width + 0.06f, height + 0.08f, 1f);

        SpriteRenderer borderRenderer = border.AddComponent<SpriteRenderer>();
        borderRenderer.sprite = sprite;
        borderRenderer.color = new Color(0.2f, 0.95f, 0.55f);
        borderRenderer.sortingOrder = 39;

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        CreateTextLayer("CodeShadowA", code, new Vector3(-0.02f, -0.02f, 0f), new Color(0f, 0f, 0f, 0.9f), 42, font);
        CreateTextLayer("CodeShadowB", code, new Vector3(0.02f, -0.02f, 0f), new Color(0f, 0f, 0f, 0.9f), 42, font);
        CreateTextLayer("CodeShadowC", code, new Vector3(-0.02f, 0.02f, 0f), new Color(0f, 0f, 0f, 0.9f), 42, font);
        CreateTextLayer("CodeShadowD", code, new Vector3(0.02f, 0.02f, 0f), new Color(0f, 0f, 0f, 0.9f), 42, font);

        Color mainColor = powerLevel > 0 ? new Color(1f, 0.95f, 0.35f) : new Color(0.2f, 1f, 0.65f);
        CreateTextLayer("CodeMain", code, new Vector3(0f, -0.01f, 0f), mainColor, 43, font);

        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void CreateTextLayer(string name, string text, Vector3 localPos, Color color, int sortingOrder, Font font)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = localPos;

        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.characterSize = 0.11f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = color;
        textMesh.fontSize = 74;

        if (font != null)
        {
            textMesh.font = font;
            textObj.GetComponent<MeshRenderer>().sharedMaterial = font.material;
        }

        MeshRenderer meshRenderer = textObj.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = sortingOrder;
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
