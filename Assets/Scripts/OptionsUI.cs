using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Options buttons")]
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [Header("Options texts")]
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [Header("Controls buttons")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [Header("Controls texts")]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [Header("Rebind")]
    [SerializeField] private Transform pressToRebindTransform;




    public static OptionsUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
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


        // Controlls rebinding
        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternative));
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));

    }

    private void Start()
    {
        GameManager.Instance.OnPaused += GameManager_OnPaused;
        UpdateVisual();

        Show(false);
        ShowPressToRebind(false);
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

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternative);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

    }

    internal void Show(bool v)
    {
        gameObject.SetActive(v);
    }

    private void ShowPressToRebind(bool v)
    {
        pressToRebindTransform.gameObject.SetActive(v);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebind(true);
        GameInput.Instance.RebindBinding(binding, () =>
        {
            ShowPressToRebind(false);
            UpdateVisual();
        });
    }

    private void GameManager_OnPaused(bool v)
    {
        if (!v)
        {
            Show(false);
        }
    }
}
