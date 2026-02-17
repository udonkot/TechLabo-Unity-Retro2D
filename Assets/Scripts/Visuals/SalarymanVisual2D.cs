using UnityEngine;

public class SalarymanVisual2D : MonoBehaviour
{
    private Transform visualRoot;

    public Transform VisualRoot => visualRoot;

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        if (transform.Find("VisualRoot") != null)
        {
            visualRoot = transform.Find("VisualRoot");
            return;
        }

        SpriteRenderer baseRenderer = GetComponent<SpriteRenderer>();
        if (baseRenderer != null)
        {
            baseRenderer.enabled = false;
        }

        Sprite sprite = CreateSolidSprite();

        GameObject root = new GameObject("VisualRoot");
        root.transform.SetParent(transform);
        root.transform.localPosition = new Vector3(0f, -0.02f, 0f);
        visualRoot = root.transform;

        CreatePart(visualRoot, "LegLeft", sprite, new Vector2(-0.15f, -0.37f), new Vector2(0.18f, 0.42f), new Color(0.08f, 0.08f, 0.12f), 20);
        CreatePart(visualRoot, "LegRight", sprite, new Vector2(0.15f, -0.37f), new Vector2(0.18f, 0.42f), new Color(0.08f, 0.08f, 0.12f), 20);
        CreatePart(visualRoot, "Body", sprite, new Vector2(0f, -0.02f), new Vector2(0.5f, 0.62f), new Color(0.2f, 0.3f, 0.7f), 22);
        CreatePart(visualRoot, "Shirt", sprite, new Vector2(0f, 0.02f), new Vector2(0.26f, 0.34f), new Color(0.92f, 0.94f, 0.98f), 23);
        CreatePart(visualRoot, "Tie", sprite, new Vector2(0f, -0.04f), new Vector2(0.08f, 0.26f), new Color(0.74f, 0.1f, 0.12f), 24);
        CreatePart(visualRoot, "ArmLeft", sprite, new Vector2(-0.31f, -0.02f), new Vector2(0.12f, 0.44f), new Color(0.2f, 0.3f, 0.7f), 21);
        CreatePart(visualRoot, "ArmRight", sprite, new Vector2(0.31f, -0.02f), new Vector2(0.12f, 0.44f), new Color(0.2f, 0.3f, 0.7f), 21);
        CreatePart(visualRoot, "Laptop", sprite, new Vector2(0.2f, 0.1f), new Vector2(0.46f, 0.24f), new Color(0.35f, 0.35f, 0.38f), 25);
        CreatePart(visualRoot, "LaptopScreen", sprite, new Vector2(0.2f, 0.17f), new Vector2(0.34f, 0.1f), new Color(0.66f, 0.9f, 0.92f), 26);
        CreatePart(visualRoot, "Neck", sprite, new Vector2(0f, 0.33f), new Vector2(0.15f, 0.08f), new Color(0.98f, 0.84f, 0.68f), 26);
        CreatePart(visualRoot, "Head", sprite, new Vector2(0f, 0.54f), new Vector2(0.48f, 0.44f), new Color(0.98f, 0.84f, 0.68f), 27);
        CreatePart(visualRoot, "Hair", sprite, new Vector2(0f, 0.68f), new Vector2(0.5f, 0.16f), new Color(0.12f, 0.08f, 0.08f), 28);
        CreatePart(visualRoot, "EyeLeft", sprite, new Vector2(-0.1f, 0.55f), new Vector2(0.05f, 0.05f), Color.black, 29);
        CreatePart(visualRoot, "EyeRight", sprite, new Vector2(0.1f, 0.55f), new Vector2(0.05f, 0.05f), Color.black, 29);
    }

    private static void CreatePart(Transform parent, string name, Sprite sprite, Vector2 localPos, Vector2 localScale, Color color, int order)
    {
        GameObject part = new GameObject(name);
        part.transform.SetParent(parent);
        part.transform.localPosition = localPos;
        part.transform.localScale = new Vector3(localScale.x, localScale.y, 1f);

        SpriteRenderer renderer = part.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        renderer.sortingOrder = order;
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