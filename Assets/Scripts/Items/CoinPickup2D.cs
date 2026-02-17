using UnityEngine;

public class CoinPickup2D : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        PlayerController2D player = other.GetComponent<PlayerController2D>();
        if (player == null)
        {
            return;
        }

        collected = true;

        GameSession session = FindObjectOfType<GameSession>();
        if (session != null)
        {
            session.AddCoin();
        }

        gameObject.SetActive(false);
    }
}
