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

    private const string PPLAYER_PREFS_BINDINGS = "GameInputBindings";

    public static GameInput Instance { get; private set; }

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternative,
        Pause
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        playerInputActions = new PlayerInputActions();

        // Load saved bindings if they exist
        if (PlayerPrefs.HasKey(PPLAYER_PREFS_BINDINGS))
        {
            string savedBindings = PlayerPrefs.GetString(PPLAYER_PREFS_BINDINGS);
            playerInputActions.LoadBindingOverridesFromJson(savedBindings);
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += ctx => Interact();
        playerInputActions.Player.InteractAlternative.performed += ctx => InteractAlternative();
        playerInputActions.Player.Pause.performed += ctx => PauseGame();
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

        }).Start(); // Ensure the rebinding process starts
    }
}
