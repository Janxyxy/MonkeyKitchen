using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public event Action OnInteractAction;
    public event Action OnInteractAlternativeAction;
    public event Action OnPauseAction;

    public static GameInput Instance { get; private set; }

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
}
