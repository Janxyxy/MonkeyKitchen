using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;

    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO currentFryingRecipeSO;
    private FryingState fryingState;

    private enum FryingState
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private void Start()
    {
        fryingState = FryingState.Idle;
        fryingTimer = 0f;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (fryingState)
            {
                case FryingState.Idle:
                    break;
                case FryingState.Frying:
                    fryingTimer += Time.deltaTime;

                    if (fryingTimer > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState = FryingState.Fried;
                        burningTimer = 0f;

                        currentFryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject());
                    }
                    break;
                case FryingState.Fried:
                    burningTimer += Time.deltaTime;

                    if (burningTimer > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState = FryingState.Burned;
                    }
                    break;
                case FryingState.Burned:
                    break;
            }
        }
    }

    internal override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeForInput(player.GetKitchenObject()))
                {
                    // Player has a KitchenObject and it matches a recipe input, so we can set the StoveCounter's KitchenObject.
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    currentFryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject());

                    fryingState = FryingState.Frying;
                    fryingTimer = 0f;

                    // TODO: progress later
                }
            }
            else
            {
                // Player does not have a KitchenObject, so we cannot interact with the StoveCounter.
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                // Player already has a KitchenObject, so we cannot set the StoveCounter's KitchenObject as the player's.
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    private bool HasRecipeForInput(KitchenObject inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObject);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObject inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObject);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObject inputKitchenObject)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOs)
        {
            if (fryingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO())
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
}
