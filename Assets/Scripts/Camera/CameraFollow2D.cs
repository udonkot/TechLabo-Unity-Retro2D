using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -10f);
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private bool followY = false;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            PlayerController2D player = FindObjectOfType<PlayerController2D>();
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 desired = target.position + offset;
        if (!followY)
        {
            desired.y = transform.position.y;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    public void SetTarget(Transform followTarget)
    {
        target = followTarget;
    }
}
