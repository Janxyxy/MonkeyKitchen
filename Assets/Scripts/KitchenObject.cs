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
            this.kitchenObjectParent.ClearKitchenObject();
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

    internal void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    internal static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
