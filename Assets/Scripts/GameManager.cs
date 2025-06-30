using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState currentGameState;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gameplayTimer;
    private float gameplayTimerMax = 60f;
    private bool isGamePaused = false;


    public enum GameState
    {
        WaitingToStart,
        Countown,
        Playing,
        GameOver,
    };

    public event Action<GameState> OnStateChanged;
    public event Action<bool> OnPaused;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction()
    {
        TogglePauseGame();
    }

    internal void TogglePauseGame ()
    {
        isGamePaused = !isGamePaused;
        OnPaused?.Invoke(isGamePaused);

        if (isGamePaused)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentGameState = GameState.WaitingToStart;
    }
    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    currentGameState = GameState.Countown;
                    OnStateChanged?.Invoke(currentGameState);

                }
                break;
            case GameState.Countown:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    gameplayTimer = gameplayTimerMax;
                    currentGameState = GameState.Playing;
                    OnStateChanged?.Invoke(currentGameState);
                }
                break;
            case GameState.Playing:
                gameplayTimer -= Time.deltaTime;
                if (gameplayTimer <= 0f)
                {
                    currentGameState = GameState.GameOver;
                    OnStateChanged?.Invoke(currentGameState);

                }
                break;
            case GameState.GameOver:
                OnStateChanged?.Invoke(currentGameState);
                break;
        }
    }

    internal bool IsGamePlaing()
    {
        return currentGameState == GameState.Playing;
    }

    internal bool IsGameOver()
    {
        return currentGameState == GameState.GameOver;
    }

    internal float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    internal float GetGameplayTimerNormalized()
    {
        return 1 - gameplayTimer / gameplayTimerMax;
    }
}
