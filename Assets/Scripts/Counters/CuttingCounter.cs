using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;

    public event Action<float> OnProgressChanged;

    public event Action OnCut;
    public event Action OnCutFinal;

    public static event Action<Transform> OnAnyCut;

    new internal static void ResetStaticData()
    {
        OnAnyCut = null;
    }

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
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoWithInput(GetKitchenObject());
                    OnProgressChanged.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
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
            cuttingProgress++;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoWithInput(GetKitchenObject());

            OnProgressChanged.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);

            OnAnyCut?.Invoke(this.transform);

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                OnCutFinal?.Invoke();
                KitchenObjectSO outputKitchenObject = GetOutputForInput(GetKitchenObject());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
            }
            else
            {
                OnCut?.Invoke();
            }
        }
    }

    private bool HasRecipeForInput(KitchenObject inputKitchenObject)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoWithInput(inputKitchenObject);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObject inputKitchenObject)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoWithInput(inputKitchenObject);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSoWithInput(KitchenObject inputKitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs)
        {
            if (cuttingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO())
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
