using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    internal override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Only accept a plate
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }       
        }
    }
}
