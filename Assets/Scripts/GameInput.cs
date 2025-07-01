using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private PlayerInputActions playerInputActions;

    public event Action OnInteractAction;
    public event Action OnInteractAlternativeAction;
    public event Action OnPauseAction;
    public event Action OnBindingRebind;

    private const string PPLAYER_PREFS_BINDINGS = "GameInputBindings";

    public static GameInput Instance { get; private set; }

    private ControlScheme currentControlScheme;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternative,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternative,
        Gamepad_Pause
    }

    public enum ControlScheme
    {
        Keyboard,
        Gamepad
    }

    private void Awake()
    {
        if (Instance != null)
        { Destroy(gameObject); return; }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        void Register(InputAction action, Action callback)
        {
            action.performed += ctx =>
            {
                UpdateControlScheme(ctx.control.device); 
                callback?.Invoke();
            };
        }

        Register(playerInputActions.Player.Interact, Interact);
        Register(playerInputActions.Player.InteractAlternative, InteractAlternative);
        Register(playerInputActions.Player.Pause, PauseGame);
        Register(playerInputActions.Player.Move, null); 
    }

    private void UpdateControlScheme(InputDevice device)
    {
        var scheme = device is Gamepad ? ControlScheme.Gamepad : ControlScheme.Keyboard;
        if (scheme != currentControlScheme)
        {
            currentControlScheme = scheme;
            if (debugMode)
                Debug.Log($"Switched to {currentControlScheme}");
        }
    }

    private void PauseGame()
    {
        OnPauseAction?.Invoke();
    }

    private void Interact()
    {
        OnInteractAction?.Invoke();
    }

    private void InteractAlternative()
    {
        OnInteractAlternativeAction?.Invoke();
    }

    internal Vector2 GetMovementVectorNormalized()
    {

        Vector2 intputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        intputVector = intputVector.normalized;

        return intputVector;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= ctx => Interact();
        playerInputActions.Player.InteractAlternative.performed -= ctx => InteractAlternative();
        playerInputActions.Player.Pause.performed -= ctx => PauseGame();

        playerInputActions.Dispose();
    }

    internal string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            // Keyboard bindings
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternative:
                return playerInputActions.Player.InteractAlternative.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();

            // Gamepad bindings
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternative:
                return playerInputActions.Player.InteractAlternative.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();

            default:
                throw new ArgumentOutOfRangeException(nameof(binding), binding, null);
        }
    }

    internal void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
                inputAction = null;
                bindingIndex = -1;

                Debug.LogError("RebindBinding called with an invalid binding: " + binding);
                break;
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternative:
                inputAction = playerInputActions.Player.InteractAlternative;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternative:
                inputAction = playerInputActions.Player.InteractAlternative;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;

        }

        if (debugMode)
        {
            Debug.Log("Rebinding: " + binding.ToString());
            Debug.Log("Binding index: " + bindingIndex);
            Debug.Log("Input action: " + inputAction.name);
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            if (debugMode)
            {
                Debug.Log("Rebinding complete: " + binding.ToString());
                Debug.Log("New binding path: " + callback.action.bindings[bindingIndex].effectivePath);
            }
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound?.Invoke();

            PlayerPrefs.SetString(PPLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke();

        }).Start(); // Ensure the rebinding process starts
    }


    internal bool IsUsingKeyboard() => currentControlScheme == ControlScheme.Keyboard;
}
