using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    internal override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeForInput(player.GetKitchenObject()))
                {
                    // Player has a KitchenObject and it matches a recipe input, so we can set the CuttingCounter's KitchenObject.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
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
        if (HasKitchenObject() && HasRecipeForInput(GetKitchenObject()))
        {
            KitchenObjectSO outputKitchenObject = GetOutputForInput(GetKitchenObject());

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
        }
    }

    private bool HasRecipeForInput(KitchenObject inputKitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs)
        {
            if (cuttingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO())
            {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObject inputKitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs)
        {
            if (cuttingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO())
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}
