using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    internal override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player does not have a KitchenObject, so we cannot interact with the ClearCounter.
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                // Player already has a KitchenObject, so we cannot set the ClearCounter's KitchenObject as the player's.
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    internal override void InteractAlternative(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
        }
    }
}
