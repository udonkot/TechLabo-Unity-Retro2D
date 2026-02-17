using UnityEngine;
using UnityEngine.UI;

public class HudText2D : MonoBehaviour
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text codeText;

    private GameSession session;
    private CodeShooter2D shooter;

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

        if (shooter == null)
        {
            shooter = FindObjectOfType<CodeShooter2D>();
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

        if (codeText != null && shooter != null)
        {
            codeText.text = $"CODE {shooter.CurrentCode}  PWR {shooter.PowerLevel}";
        }
    }

    public void SetTexts(Text coin, Text score, Text lives)
    {
        coinText = coin;
        scoreText = score;
        livesText = lives;
    }

    public void SetCodeText(Text code)
    {
        codeText = code;
    }
}
