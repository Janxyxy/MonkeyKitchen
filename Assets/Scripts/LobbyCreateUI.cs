using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicLobby;
    [SerializeField] private Button createPrivateLobby;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        createPrivateLobby.onClick.AddListener(async () =>
        {
            await GameLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
        });

        createPublicLobby.onClick.AddListener(async () =>
         {
             await GameLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
         });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });


    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
