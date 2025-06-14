using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;


    private GameInput gameInput;
    private bool isMoving;

    private void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();
    }

    private void Update()
    {
        Vector2 intputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(intputVector.x, 0, intputVector.y);

        isMoving = intputVector != Vector2.zero;

        //transform.Translate(moveDir * Time.deltaTime, Space.World);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // Rotate the player to face the direction of movement
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
    }

    internal bool IsMoving()
    {
        return isMoving;
    }
}
