using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    private void Start()
    {
        stoveCounter.OnFryingStateChanged += HandleFryingStateChanged;
    }

    private void HandleFryingStateChanged(StoveCounter.FryingState state)
    {
        bool playSound = state == StoveCounter.FryingState.Frying || state == StoveCounter.FryingState.Fried;

        if (playSound && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!playSound && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
