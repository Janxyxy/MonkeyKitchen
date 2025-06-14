using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private bool isMoving;

    private void Update()
    {
        Vector2 intputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            intputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            intputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            intputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            intputVector.x += 1;
        }

        intputVector = intputVector.normalized; // Normalize the vector to ensure consistent speed

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
