using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int startLives = 3;
    [SerializeField] private float respawnDelay = 1.2f;

    [Header("Score")]
    [SerializeField] private int coinScore = 100;

    private int lives;
    private int coins;
    private int score;
    private bool levelCleared;
    private bool initialized;

    public static GameSession Instance { get; private set; }

    public int Lives => lives;
    public int Coins => coins;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (initialized)
        {
            return;
        }

        lives = startLives;
        initialized = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddCoin()
    {
        coins++;
        score += coinScore;
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void OnPlayerHit(PlayerController2D player)
    {
        // 被弾時の共通処理を追加したくなったらここに拡張
    }

    public void OnPlayerDied(PlayerController2D player)
    {
        if (levelCleared)
        {
            return;
        }

        lives--;
        if (lives <= 0)
        {
            lives = startLives;
            coins = 0;
            score = 0;
            StartCoroutine(RestartAfterDelay());
            return;
        }

        StartCoroutine(RestartAfterDelay());
    }

    public void OnReachGoal()
    {
        if (levelCleared)
        {
            return;
        }

        levelCleared = true;
        AddScore(1000);
        Invoke(nameof(LoadNextSceneOrRestart), 1.2f);
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextSceneOrRestart()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(next);
        }
        else
        {
            SceneManager.LoadScene(current);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCleared = false;
    }
}
