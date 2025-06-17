using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public event Action OnInteractAction;
    public event Action OnInteractAlternativeAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += ctx => Interact();
        playerInputActions.Player.InteractAlternative.performed += ctx => InteractAlternative();
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
}
