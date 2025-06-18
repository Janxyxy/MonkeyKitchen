using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;

    private Animator animator;

    private const string ANIM_PARAM_OPEN = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (containerCounter != null)
        {
            containerCounter.OnContainerCounterInteract += ContainerCounter_OnContainerCounterInteract;
        }
    }

    private void ContainerCounter_OnContainerCounterInteract()
    {
        animator.SetTrigger(ANIM_PARAM_OPEN);
    }
}
