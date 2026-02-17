using UnityEngine;

public class GoalPole2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController2D player = other.GetComponent<PlayerController2D>();
        if (player == null)
        {
            return;
        }

        GameSession session = FindObjectOfType<GameSession>();
        if (session != null)
        {
            session.OnReachGoal();
        }
    }
}
