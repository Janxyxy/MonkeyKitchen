using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event Action<Transform> OnAnyObjectDestroyed;

    internal override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectDestroyed?.Invoke(this.transform);

            //player.ClearKitchenObject();
        }
        else
        {
            Debug.Log("TrashCounter: Player has no KitchenObject to destroy.");
        }
    }
}
