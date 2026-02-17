using UnityEngine;

public class BugEnemyVisual2D : MonoBehaviour
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

        CreatePart(visualRoot, "Body", sprite, new Vector2(0f, -0.05f), new Vector2(0.62f, 0.52f), new Color(0.4f, 0.18f, 0.52f), 20);
        CreatePart(visualRoot, "BodyCore", sprite, new Vector2(0f, -0.02f), new Vector2(0.36f, 0.26f), new Color(0.2f, 0.08f, 0.28f), 21);
        CreatePart(visualRoot, "EyeLeft", sprite, new Vector2(-0.1f, 0.08f), new Vector2(0.12f, 0.12f), new Color(0.94f, 0.18f, 0.26f), 22);
        CreatePart(visualRoot, "EyeRight", sprite, new Vector2(0.1f, 0.08f), new Vector2(0.12f, 0.12f), new Color(0.94f, 0.18f, 0.26f), 22);
        CreatePart(visualRoot, "PupilLeft", sprite, new Vector2(-0.1f, 0.08f), new Vector2(0.04f, 0.04f), Color.black, 23);
        CreatePart(visualRoot, "PupilRight", sprite, new Vector2(0.1f, 0.08f), new Vector2(0.04f, 0.04f), Color.black, 23);

        CreatePart(visualRoot, "AntennaLeft", sprite, new Vector2(-0.18f, 0.28f), new Vector2(0.05f, 0.22f), new Color(0.34f, 0.14f, 0.46f), 19);
        CreatePart(visualRoot, "AntennaRight", sprite, new Vector2(0.18f, 0.28f), new Vector2(0.05f, 0.22f), new Color(0.34f, 0.14f, 0.46f), 19);
        CreatePart(visualRoot, "AntennaTipLeft", sprite, new Vector2(-0.18f, 0.4f), new Vector2(0.08f, 0.08f), new Color(0.95f, 0.35f, 0.48f), 24);
        CreatePart(visualRoot, "AntennaTipRight", sprite, new Vector2(0.18f, 0.4f), new Vector2(0.08f, 0.08f), new Color(0.95f, 0.35f, 0.48f), 24);

        CreatePart(visualRoot, "LegL1", sprite, new Vector2(-0.33f, -0.15f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
        CreatePart(visualRoot, "LegL2", sprite, new Vector2(-0.33f, -0.03f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
        CreatePart(visualRoot, "LegL3", sprite, new Vector2(-0.33f, 0.09f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
        CreatePart(visualRoot, "LegR1", sprite, new Vector2(0.33f, -0.15f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
        CreatePart(visualRoot, "LegR2", sprite, new Vector2(0.33f, -0.03f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
        CreatePart(visualRoot, "LegR3", sprite, new Vector2(0.33f, 0.09f), new Vector2(0.12f, 0.06f), new Color(0.18f, 0.08f, 0.26f), 18);
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