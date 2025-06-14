using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float playerRadus = 1f;
    [SerializeField] private float playerHeight = 2f;


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

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = CanMove(moveDir, moveDistance);

        if (!canMove)
        {
            // Attemt to move in the X direction
            Vector3 xMoveDir = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = CanMove(xMoveDir, moveDistance);

            if (canMove)
            {
                moveDir = xMoveDir;
            }
            else
            {
                // Attempt to move in the Z direction
                Vector3 zMoveDir = new Vector3(0, 0, moveDir.z).normalized;
                canMove = CanMove(zMoveDir, moveDistance);
                if (canMove)
                {
                    moveDir = zMoveDir;
                }
            }
        }

        if (canMove)
        {
            //transform.Translate(moveDir * Time.deltaTime, Space.World);
            transform.position += moveDir * moveDistance;
            isMoving = intputVector != Vector2.zero;
        }

        // Rotate the player to face the direction of movement
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
    }

    private bool CanMove(Vector3 moveDir, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadus, moveDir, moveDistance);
    }

    internal bool IsMoving()
    {
        return isMoving;
    }
}
