using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour
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

        //transform.SetParent(kitchenObjectParent.GetKitchenObjectFollowTransform());
        //transform.localPosition = Vector3.zero; 
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

    internal static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }

    internal bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        plateKitchenObject = null;
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        return false;
    }
}
