using UnityEngine;

public class RetroPixelSnap2D : MonoBehaviour
{
    [SerializeField] private float pixelsPerUnit = 16f;
    [SerializeField] private bool affectChildren = false;

    private void LateUpdate()
    {
        if (pixelsPerUnit <= 0f)
        {
            return;
        }

        if (affectChildren)
        {
            foreach (Transform child in transform)
            {
                SnapTransform(child);
            }
        }
        else
        {
            SnapTransform(transform);
        }
    }

    private void SnapTransform(Transform target)
    {
        Vector3 p = target.position;
        float unit = 1f / pixelsPerUnit;
        p.x = Mathf.Round(p.x / unit) * unit;
        p.y = Mathf.Round(p.y / unit) * unit;
        target.position = p;
    }
}
