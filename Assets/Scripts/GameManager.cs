using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    private NetworkVariable<GameState> currentGameState = new NetworkVariable<GameState>(GameState.WaitingToStart);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gameplayTimer = new NetworkVariable<float>(0f);
    private float gameplayTimerMax = 90f; 
    private bool isGamePaused = false;

    private Dictionary<ulong, bool> PlayerReadyDictionary;


    public enum GameState
    {
        WaitingToStart,
        Countown,
        Playing,
        GameOver,
    };

    public event Action<GameState> OnStateChanged;
    public event Action<bool> OnPaused;
    public event Action<bool> OnLocalPlayerReadyChanged;

    private bool isLocalPlayerReady;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        PlayerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        currentGameState.OnValueChanged += CurrentGameState_OnValueChanged;
    }
    private void CurrentGameState_OnValueChanged(GameState previousValue, GameState newValue)
    {
        OnStateChanged?.Invoke(currentGameState.Value);
    }

    private void GameInput_OnInteractAction()
    {
        if (currentGameState.Value == GameState.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(isLocalPlayerReady);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientId = serverRpcParams.Receive.SenderClientId;
        PlayerReadyDictionary[senderClientId] = true;

        bool allclientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!PlayerReadyDictionary.ContainsKey(clientId) || !PlayerReadyDictionary[clientId])
            {
                allclientsReady = false;
                return;
            }
        }

        if (allclientsReady)
        {
            currentGameState.Value = GameState.Countown;
        }
    }

    private void GameInput_OnPauseAction()
    {
        TogglePauseGame();
    }

    internal void TogglePauseGame()
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

        currentGameState.Value = GameState.WaitingToStart;
    }
    private void Update()
    {
        if (!IsServer)
            return;

        switch (currentGameState.Value)
        {
            case GameState.WaitingToStart:

                break;
            case GameState.Countown:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value <= 0f)
                {
                    gameplayTimer.Value = gameplayTimerMax;
                    currentGameState.Value = GameState.Playing;
                }
                break;
            case GameState.Playing:
                gameplayTimer.Value -= Time.deltaTime;
                if (gameplayTimer.Value <= 0f)
                {
                    currentGameState.Value = GameState.GameOver;

                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    internal bool IsGamePlaing()
    {
        return currentGameState.Value == GameState.Playing;
    }

    internal bool IsGameOver()
    {
        return currentGameState.Value == GameState.GameOver;
    }

    internal float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }
    internal float GetGameplayTimerNormalized()
    {
        return 1 - gameplayTimer.Value / gameplayTimerMax;
    }
}
