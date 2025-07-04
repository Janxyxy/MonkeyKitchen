using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;
    private Player player;

    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (player != null)
        {
            animator.SetBool(IS_WALKING, player.IsMoving());
        }  
    }
}
