using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() => StartHost());
        startClientButton.onClick.AddListener(() => StartClient());
    }

    private void StartHost()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("Already hosting!");
            return;
        }
        NetworkManager.Singleton.StartHost();
        Show(false);
        Debug.Log("Started hosting");
    }
    private void StartClient()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("Already hosting!");
            return;
        }
        NetworkManager.Singleton.StartClient();
        Show(false);
        Debug.Log("Started client");
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }
}
