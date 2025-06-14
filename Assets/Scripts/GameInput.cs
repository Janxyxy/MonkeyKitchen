using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    internal Vector2 GetMovementVectorNormalized()
    {

        Vector2 intputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        intputVector = intputVector.normalized;

        return intputVector;
    }
}
