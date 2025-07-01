using System;
using TMPro;
using Unity.VisualScripting;
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

    [Header("Keyboard Controls buttons")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [Header("Keyboard Controls texts")]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [Header("Gamepad Controls buttons")]
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;

    [Header("Gamepad Controls texts")]
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;


    [Header("Rebind")]
    [SerializeField] private Transform pressToRebindTransform;


   private WarningUI warningUI;

    private Action onCloseButtonAction;

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
            Hide();
            onCloseButtonAction();
        });

        warningUI = FindFirstObjectByType<WarningUI>();

        // Controlls keyboard rebinding
        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternative));
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));

        // Controlls gamepad rebinding
        gamepadInteractButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Gamepad_Interact));
        gamepadInteractAltButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Gamepad_InteractAlternative));
        gamepadPauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Gamepad_Pause));

    }

    private void Start()
    {
        GameManager.Instance.OnPaused += GameManager_OnPaused;
        UpdateVisual();

        Hide();
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

        // Keyboard Controls texts
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternative);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        // Gamepad Controls texts
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternative);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);

    }

    internal void Show(Action oncloseButtonAction)
    {
        gameObject.SetActive(true);

        soundEffectsButton.Select();
        this.onCloseButtonAction = oncloseButtonAction;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebind(bool v)
    {
        pressToRebindTransform.gameObject.SetActive(v);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        if (!GameInput.Instance.IsUsingKeyboard() && IsKeyboardBinding(binding))
        {
            Debug.LogWarning("Cannot rebind keyboard bindings while using a gamepad.");
            warningUI.ShowWarning("Cannot rebind keyboard bindings while using a gamepad.", 2.5f);
            return;
        }
        else if (GameInput.Instance.IsUsingKeyboard() && IsGamepadBinding(binding))
        {
            Debug.LogWarning("Cannot rebind gamepad bindings while using a keyboard.");
            warningUI.ShowWarning("Cannot rebind gamepad bindings while using a keyboard.", 2.5f);
            return;
        }

        ShowPressToRebind(true);
        GameInput.Instance.RebindBinding(binding, () =>
        {
            ShowPressToRebind(false);
            UpdateVisual();
        });
    }

    private bool IsKeyboardBinding(GameInput.Binding binding)
    {
        return binding == GameInput.Binding.Move_Up ||
               binding == GameInput.Binding.Move_Down ||
               binding == GameInput.Binding.Move_Left ||
               binding == GameInput.Binding.Move_Right ||
               binding == GameInput.Binding.Interact ||
               binding == GameInput.Binding.InteractAlternative ||
               binding == GameInput.Binding.Pause;
    }

    private bool IsGamepadBinding(GameInput.Binding binding)
    {
        return binding == GameInput.Binding.Gamepad_Interact ||
               binding == GameInput.Binding.Gamepad_InteractAlternative ||
               binding == GameInput.Binding.Gamepad_Pause;
    }

    private void GameManager_OnPaused(bool v)
    {
        if (!v)
        {
            Hide();
        }
    }
}
