using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameLobby : MonoBehaviour
{
    private Lobby joinedLobby;

    private float heartbeatTimer;
    private float listLobbiesTimer;

    public event Action OnCreteLobbyStarted;
    public event Action OnCreteLobbyFaild;
    public event Action OnJoinStarted;
    public event Action OnQuickJoinFailed;
    public event Action OnJoinFailed;

    public event Action<List<Lobby>> OnLobbyListChanged;


    public static GameLobby Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeUnityAuthentication();
    }
    private void Update()
    {
        HandleHeartBeat();
        HandlePeriodic();
    }

    private void HandlePeriodic()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn && SceneManager.GetActiveScene().name == Loader.Scene.Lobby.ToString())
        {
            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0f)
            {
                float listLobbiesTimerMax = 5f;
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobby();
            }
        }
    }

    private void HandleHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            float heartbeatTimerMax = 15f;
            if (heartbeatTimer <= 0f)
            {
                heartbeatTimer = heartbeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }

        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && AuthenticationService.Instance.PlayerId == joinedLobby.HostId;
    }

    private async void ListLobby()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
            {
                // Only lobbies with available slots
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
            }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbyListChanged?.Invoke(queryResponse.Results);

            Debug.Log($"List Lobby Success: {queryResponse.Results.Count} lobbies found");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"List Lobby Failed: {e}");
        }
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 999999).ToString());

            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async Task CreateLobby(string lobbyName, bool isPrivate)
    {
        OnCreteLobbyStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KitchenGameMultiplayer.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate
            });

            KitchenGameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelect);

        }
        catch (LobbyServiceException e)
        {
            OnCreteLobbyFaild?.Invoke();
            Debug.LogError($"Create Lobby Failed: {e}");
        }
    }
    public async void QuickJoinLobby()
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Quick Join Lobby Failed: {e}");
            OnQuickJoinFailed?.Invoke();
        }
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }

    internal async Task JoinLobbyByCode(string lobbyCode)
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Join Lobby By Code Failed: {e}");
            OnJoinFailed?.Invoke();
            return;
        }
    }

    internal async Task JoinLobbyById(string lobbyId)
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Join Lobby By Id Failed: {e}");
            OnJoinFailed?.Invoke();
            return;
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Leave Lobby Failed: {e}");
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Leave Lobby Failed: {e}");
            }
        }

    }

    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Delete Lobby Failed: {e}");
            }
        }
    }
}
