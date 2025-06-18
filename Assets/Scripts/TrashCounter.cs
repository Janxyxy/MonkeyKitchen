using UnityEngine;

public class TrashCounter : BaseCounter
{
    internal override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            //player.ClearKitchenObject();
        }
        else
        {
            Debug.Log("TrashCounter: Player has no KitchenObject to destroy.");
        }
    }
}
