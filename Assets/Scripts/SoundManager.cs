using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailure += DeliveryManager_OnRecipeFailure;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedOnCounter += BaseCounter_OnAnyObjectPlacedOnCounter;

        TrashCounter.OnAnyObjectDestroyed += TrashCounter_OnAnyObjectTrashed;

    }

    private void TrashCounter_OnAnyObjectTrashed(Transform transform)
    {
        PlaySound(audioClipsRefsSO.trash, transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedOnCounter(Transform transform)
    {
        PlaySound(audioClipsRefsSO.objectDrop, transform.position);
    }

    private void Player_OnPickSomething(Transform transform)
    {
        PlaySound(audioClipsRefsSO.objectPickup, transform.position);
    }

    private void CuttingCounter_OnAnyCut(Transform transform)
    {
        PlaySound(audioClipsRefsSO.chop, transform.position);
    }


    private void DeliveryManager_OnRecipeFailure()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipsArray, Vector3 position, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(audioClipsArray[UnityEngine.Random.Range(0, audioClipsArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1)
    {
       AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
