using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAltText;
    [SerializeField] private TextMeshProUGUI keyPauseText;

    [SerializeField] private TextMeshProUGUI keyInteractGamepadText;
    [SerializeField] private TextMeshProUGUI keyInteractAltGamepadText;
    [SerializeField] private TextMeshProUGUI keyPauseGamepadText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += UpdateVisual;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();
        Show(true);
    }

    private void GameManager_OnStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Countown)
        {
            Show(false);
        }
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternative);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyInteractGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        keyInteractAltGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternative);
        keyPauseGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }
}
