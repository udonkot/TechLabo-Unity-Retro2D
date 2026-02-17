using UnityEngine;

public class CodePowerUp2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        CodeShooter2D shooter = other.GetComponent<CodeShooter2D>();
        if (shooter == null)
        {
            return;
        }

        shooter.LevelUp();
        gameObject.SetActive(false);
    }
}
