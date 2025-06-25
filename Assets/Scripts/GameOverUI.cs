using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredCount;

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
