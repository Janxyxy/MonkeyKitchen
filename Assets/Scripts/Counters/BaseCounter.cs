using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public static event Action<Transform> OnAnyObjectPlacedOnCounter;

    internal static void ResetStaticData()
    {
        OnAnyObjectPlacedOnCounter = null;
    }

    internal virtual void Interact(Player player)
    {
        Debug.LogError($"{gameObject.name} does not have an Interact method implemented.");
    }

    internal virtual void InteractAlternative(Player player)
    {
        // Not all counters will implement an alternative interaction
        //Debug.LogError($"{gameObject.name} does not have an InteractAlternative method implemented.");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlacedOnCounter?.Invoke(counterTopPoint);
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
}
