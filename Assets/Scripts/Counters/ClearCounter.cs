using UnityEngine;

public class ClearCounter : BaseCounter
{
    //[SerializeField] private KitchenObjectSO kitchenObjectSO;


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
                PlateKitchenObject plateKitchenObject;
                // Player already has a KitchenObject, so we cannot set the ClearCounter's KitchenObject as the player's.
                if (player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
