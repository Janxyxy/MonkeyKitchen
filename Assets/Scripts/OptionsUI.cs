using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    public static OptionsUI Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        soundEffectsButton.onClick.AddListener(OnSoundEffectsButtonClicked);
        musicButton.onClick.AddListener(OnMusicButtonClicked);
        closeButton.onClick.AddListener(() =>
        {
            Show(false);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnPaused += GameManager_OnPaused;
        UpdateVisual();
        Show(false);
    }

    private void OnMusicButtonClicked()
    {
        MusicManager.Instance.ChangeVolume();
        UpdateVisual();
    }

    private void OnSoundEffectsButtonClicked()
    {
        SoundManager.Instance.ChangeVolume();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Math.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Math.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    internal void Show(bool v)
    {
       gameObject.SetActive(v);
    }

    private void GameManager_OnPaused(bool v)
    {
        if (!v)
        {
            Show(false);
        }
    }

}
