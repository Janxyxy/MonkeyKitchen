using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        countdownText.text = "";
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        Show(state == GameManager.GameState.Countown);
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }

    private void Update()
    {
        countdownText.text = Math.Ceiling(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }
}
