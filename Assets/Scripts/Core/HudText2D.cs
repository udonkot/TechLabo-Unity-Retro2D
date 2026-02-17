using UnityEngine;
using UnityEngine.UI;

public class HudText2D : MonoBehaviour
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private GameSession session;

    private void Start()
    {
        session = FindObjectOfType<GameSession>();
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (session == null)
        {
            session = FindObjectOfType<GameSession>();
            if (session == null)
            {
                return;
            }
        }

        if (coinText != null)
        {
            coinText.text = $"COIN {session.Coins:00}";
        }

        if (scoreText != null)
        {
            scoreText.text = $"SCORE {session.Score:000000}";
        }

        if (livesText != null)
        {
            livesText.text = $"LIFE {session.Lives}";
        }
    }

    public void SetTexts(Text coin, Text score, Text lives)
    {
        coinText = coin;
        scoreText = score;
        livesText = lives;
    }
}
