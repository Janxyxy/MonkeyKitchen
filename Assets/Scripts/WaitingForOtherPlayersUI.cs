using System;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
        Show(false);
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Countown || state == GameManager.GameState.Playing)
        {
            Show(false);
        }
    }

    private void GameManager_OnLocalPlayerReadyChanged(bool ready)
    {
        Show(ready);
    }

    private void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
