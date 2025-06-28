using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnPaused += GameManager_OnPaused;
        Show(false);
    }

    private void GameManager_OnPaused(bool v)
    {
        Show(v);
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);

    }
}
