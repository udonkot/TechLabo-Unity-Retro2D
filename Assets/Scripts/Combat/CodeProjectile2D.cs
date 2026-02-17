using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CodeProjectile2D : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2.2f;

    private Vector2 moveDirection = Vector2.right;

    public void Initialize(Vector2 direction, string codeText)
    {
        moveDirection = direction.normalized;
        CreateVisual(codeText);
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
            Destroy(gameObject);
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

    private void CreateVisual(string codeText)
    {
        Sprite sprite = CreateSolidSprite();

        GameObject body = new GameObject("Body");
        body.transform.SetParent(transform);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(0.65f, 0.24f, 1f);

        SpriteRenderer bodyRenderer = body.AddComponent<SpriteRenderer>();
        bodyRenderer.sprite = sprite;
        bodyRenderer.color = new Color(0.15f, 0.95f, 0.55f);
        bodyRenderer.sortingOrder = 40;

        GameObject textObj = new GameObject("CodeText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0f, 0f, -0.01f);

        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = codeText;
        textMesh.characterSize = 0.15f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = new Color(0.05f, 0.12f, 0.1f);
        textMesh.fontSize = 36;
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
