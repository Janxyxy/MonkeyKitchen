using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerClountText;

    private Lobby lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(async() =>
        {
             await GameLobby.Instance.JoinLobbyById(lobby.Id);
        });
    }

    public void SetLobby(Lobby lobby)
    {
        this.lobby = lobby;
        SetLobbyName(lobby.Name);
        SetPlayerCount(lobby.Players.Count, lobby.MaxPlayers);
    }

    private void SetLobbyName(string lobbyName)
    {
        lobbyNameText.text = lobbyName;
    }

    private void SetPlayerCount(int playerCount, int maxPlayers)
    {
        playerClountText.text = playerCount + "/" + maxPlayers;
    }
}
