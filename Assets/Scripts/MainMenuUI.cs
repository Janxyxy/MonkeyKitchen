using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button repoButton;

    private const string repoURL = "https://github.com/Janxyxy/MonkeyKitchen";

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        repoButton.onClick.AddListener(OnRepoButtonClicked);

        Time.timeScale = 1f; // Ensure time scale is reset when entering the main menu
    }

    private void OnRepoButtonClicked()
    {
        // Open the repository URL in the default web browser
        Application.OpenURL(repoURL);
    }

    private void OnPlayButtonClicked()
    {
       Loader.Load(Loader.Scene.Game);
    }
    private void OnQuitButtonClicked()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
