using System;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnMultiplayerGamePaused += GameManager_OnMultiplayerGamePaused;

        Show(false);
    }

    private void GameManager_OnMultiplayerGamePaused(bool show)
    {
        Show(show);
    }

    private void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
