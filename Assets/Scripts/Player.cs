using System;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float playerRadus = 1f;
    [SerializeField] private float playerHeight = 2f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2;
    [SerializeField] private LayerMask interactLayerMask;


    private bool isMoving;
    private Vector3 lastIteractDir;
    private BaseCounter selectedClearCounter;

    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public event Action<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs
    {
        public BaseCounter selectedCounter;
    }

    public event Action<Transform> OnPickSomething;

    //public static Player Instance { get; private set; }

    private void Awake()
    {
        //Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction;
    }


    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
        HandleInteractins();
    }


    private void HandleMovement()
    {
        Vector2 intputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(intputVector.x, 0, intputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = CanMove(moveDir, moveDistance);

        float movementThreshold = 0.5f;

        if (!canMove)
        {
            // Attemt to move in the X direction
            Vector3 xMoveDir = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = (moveDir.x < -movementThreshold || moveDir.x > movementThreshold) && CanMove(xMoveDir, moveDistance);

            if (canMove)
            {
                moveDir = xMoveDir;
            }
            else
            {
                // Attempt to move in the Z direction
                Vector3 zMoveDir = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -movementThreshold || moveDir.z > movementThreshold) && CanMove(zMoveDir, moveDistance);
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

    private void HandleInteractins()
    {
        Vector2 intputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(intputVector.x, 0, intputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastIteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastIteractDir, out RaycastHit hit, interactDistance, interactLayerMask))
        {
            if (hit.transform != null && hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedClearCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void GameInput_OnInteractAction()
    {
        if (!GameManager.Instance.IsGamePlaing())
            return;

        if (selectedClearCounter != null)
        {
            selectedClearCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternativeAction()
    {
        if (!GameManager.Instance.IsGamePlaing())
            return;

        if (selectedClearCounter != null)
        {
            selectedClearCounter.InteractAlternative(this);
        }
    }

    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        this.selectedClearCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(new OnSelectedCounterChangedEventArgs { selectedCounter = selectedClearCounter });

    }


    private bool CanMove(Vector3 moveDir, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadus, moveDir, moveDistance);
    }

    internal bool IsMoving()
    {
        return isMoving;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke(this.transform);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    /*
       private void HandleMovementServerAuth()
    {
        Vector2 intputVector = GameInput.Instance.GetMovementVectorNormalized();
        HandleMovementServerRpc(intputVector);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleMovementServerRpc(Vector2 intputVector)
    {
        Vector3 moveDir = new Vector3(intputVector.x, 0, intputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = CanMove(moveDir, moveDistance);

        float movementThreshold = 0.5f;

        if (!canMove)
        {
            // Attemt to move in the X direction
            Vector3 xMoveDir = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = (moveDir.x < -movementThreshold || moveDir.x > movementThreshold) && CanMove(xMoveDir, moveDistance);

            if (canMove)
            {
                moveDir = xMoveDir;
            }
            else
            {
                // Attempt to move in the Z direction
                Vector3 zMoveDir = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -movementThreshold || moveDir.z > movementThreshold) && CanMove(zMoveDir, moveDistance);
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
    */
}
