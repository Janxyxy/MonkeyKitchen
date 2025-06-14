using UnityEngine;

public class PlayerAnimator : MonoBehaviour
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
        animator.SetBool(IS_WALKING, player.IsMoving());
    }
}
