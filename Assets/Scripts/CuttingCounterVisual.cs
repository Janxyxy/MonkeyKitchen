using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    private const string FINAL = "Final";

    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += HandleCuttingCounterOnCut;
        cuttingCounter.OnCutFinal += HandleCuttingCounterOnCutFinal;
    }

    private void HandleCuttingCounterOnCutFinal()
    {
        animator.SetTrigger(FINAL);
    }

    private void HandleCuttingCounterOnCut()
    {
        animator.SetTrigger(CUT);
    }
}
