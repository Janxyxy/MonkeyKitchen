using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGO;
    [SerializeField] private GameObject particlesGO;

    private void Start()
    {
        stoveCounter.OnFryingStateChanged += StoveCounter_OnFryingStateChanged;
        particlesGO.SetActive(false);
        stoveOnGO.SetActive(false);
    }

    private void StoveCounter_OnFryingStateChanged(StoveCounter.FryingState state)
    {
        bool showVisual = state == StoveCounter.FryingState.Frying || state == StoveCounter.FryingState.Fried;
        particlesGO.SetActive(showVisual);
        stoveOnGO.SetActive(showVisual);
    }
}
