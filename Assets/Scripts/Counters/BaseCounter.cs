using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

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
