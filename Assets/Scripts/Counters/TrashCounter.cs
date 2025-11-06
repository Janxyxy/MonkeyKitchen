using System;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event Action<Transform> OnAnyObjectDestroyed;

    new internal static void ResetStaticData()
    {
        OnAnyObjectDestroyed = null;
    }

    internal override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());

            InteractLogicServerRpc();
        }
        else
        {
            Debug.Log("TrashCounter: Player has no KitchenObject to destroy.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyObjectDestroyed?.Invoke(this.transform);
    }
}
