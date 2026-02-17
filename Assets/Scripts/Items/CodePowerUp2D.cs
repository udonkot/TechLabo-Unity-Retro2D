using UnityEngine;

public class CodePowerUp2D : MonoBehaviour
{
    [SerializeField] private string token = "=";

    private void Start()
    {
        SyncLabel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CodeShooter2D shooter = other.GetComponent<CodeShooter2D>();
        if (shooter == null)
        {
            shooter = other.GetComponentInParent<CodeShooter2D>();
        }

        if (shooter == null)
        {
            return;
        }

        shooter.AppendToken(token);
        transform.localScale *= 1.3f;
        gameObject.SetActive(false);
    }

    public void SetToken(string value)
    {
        token = value;
        SyncLabel();
    }

    private void SyncLabel()
    {
        TextMesh label = GetComponentInChildren<TextMesh>();
        if (label != null)
        {
            label.text = token;
        }
    }
}
