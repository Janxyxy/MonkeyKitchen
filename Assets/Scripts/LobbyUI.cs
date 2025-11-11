using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TMP_InputField playerNameInputFIeld;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(async () =>
        {
            lobbyCreateUI.Show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.QuickJoinLobby();
        });

        joinCodeButton.onClick.AddListener(async () =>
        {
            string lobbyCode = codeInputField.text;
            await GameLobby.Instance.JoinLobbyByCode(lobbyCode);
        });

    }

    private void Start()
    {
        playerNameInputFIeld.text = KitchenGameMultiplayer.Instance.GetPlayerName();
        playerNameInputFIeld.onValueChanged.AddListener((string newPlayerName) =>
        {
            KitchenGameMultiplayer.Instance.SetPlayerName(newPlayerName);
        });
    }
}
