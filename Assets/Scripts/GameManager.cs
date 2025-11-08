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

    // Pause
    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    private bool autoTestGamePausedState = false;

    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;


    public enum GameState
    {
        WaitingToStart,
        Countown,
        Playing,
        GameOver,
    };

    public event Action<GameState> OnStateChanged;
    public event Action<bool> OnLocalGamePaused;
    public event Action<bool> OnLocalPlayerReadyChanged;
    public event Action<bool> OnMultiplayerGamePaused;

    private bool isLocalPlayerReady;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        currentGameState.OnValueChanged += CurrentGameState_OnValueChanged;
        isGamePaused.OnValueChanged += IsLocalGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (playerPausedDictionary.ContainsKey(clientId))
        {
            playerPausedDictionary.Remove(clientId);
        }

        if (playerReadyDictionary.ContainsKey(clientId))
        {
            playerReadyDictionary.Remove(clientId);
        }

        autoTestGamePausedState = true;
    }

    private void IsLocalGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerGamePaused?.Invoke(true);
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerGamePaused?.Invoke(false);
        }
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
        playerReadyDictionary[senderClientId] = true;

        bool allclientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allclientsReady = false;
                break;
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
        isLocalGamePaused = !isLocalGamePaused;
        OnLocalGamePaused?.Invoke(isLocalGamePaused);

        if (isLocalGamePaused)
        {
            PauseGameServerRpc();
        }
        else
        {
            UnPauseGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("NetworkManager is null, cannot test game paused state.");
            isGamePaused.Value = false;
            return;
        }

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                // This player is paused
                isGamePaused.Value = true;
                return;
            }
        }

        // All players are unpaused
        isGamePaused.Value = false;
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

    private void LateUpdate()
    {
        if (!IsServer)
            return;

        if (autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
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
