using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredCount;
    [SerializeField] private Button playAgainButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Show(false);
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        Show(state == GameManager.GameState.GameOver);
        recipesDeliveredCount.text = DeliveryManager.Instance.GetSuccessfullRecipesDeliveredCount().ToString();
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }
}
