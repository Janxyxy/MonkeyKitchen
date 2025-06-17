using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    internal KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    internal void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectparent already has a KitchenObject!");
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.SetParent(kitchenObjectParent.GetKitchenObjectFollowTransform());
        transform.localPosition = Vector3.zero; 
    }

    internal IKitchenObjectParent GetClearCounter()
    {
        return kitchenObjectParent;
    }
}
