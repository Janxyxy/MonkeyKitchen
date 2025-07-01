using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private int previousCountdownNumber;
    private const string COUNTDOWN_POPUP = "NumberPopup";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

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
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (countdownNumber != previousCountdownNumber)
        {
            Debug.Log($"Countdown number changed: {countdownNumber}");
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(COUNTDOWN_POPUP);
            SoundManager.Instance.PlayCountDownSound();
        }
    }
}
