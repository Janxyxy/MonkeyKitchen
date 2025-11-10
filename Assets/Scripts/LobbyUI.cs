using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(async () =>
        {
            await GameLobby.Instance.CreateLobby("New Lobby", false);
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.QuickJoinLobby();
        });
    }
}
