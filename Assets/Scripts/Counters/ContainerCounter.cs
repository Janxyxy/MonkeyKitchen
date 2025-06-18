using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event Action OnContainerCounterInteract;

    internal override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnContainerCounterInteract?.Invoke();
        }
    }

    internal override void InteractAlternative(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                // Player already has a KitchenObject, so we cannot set the ContainerCounter's KitchenObject as the player's.
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnContainerCounterInteract?.Invoke();
            }
        }
    }
}
