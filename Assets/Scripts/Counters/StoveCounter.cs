using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;

    public event Action<float> OnProgressChanged;
    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO currentFryingRecipeSO;
    private FryingState fryingState;

    public event Action<FryingState> OnFryingStateChanged;

    public enum FryingState
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

                    OnProgressChanged?.Invoke(fryingTimer / currentFryingRecipeSO.fryingProgressMax);

                    if (fryingTimer > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState = FryingState.Fried;
                        burningTimer = 0f;

                        currentFryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject());

                        OnFryingStateChanged?.Invoke(fryingState);
                    }
                    break;
                case FryingState.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(burningTimer / currentFryingRecipeSO.fryingProgressMax);

                    if (burningTimer > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState = FryingState.Burned;
                        OnFryingStateChanged?.Invoke(fryingState);

                        OnProgressChanged?.Invoke(0f);
                    }
                    break;
                case FryingState.Burned:
                    OnProgressChanged?.Invoke(0f);
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

                    OnFryingStateChanged?.Invoke(fryingState);
                    OnProgressChanged?.Invoke(fryingTimer / currentFryingRecipeSO.fryingProgressMax);
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
                fryingState = FryingState.Idle;

                OnFryingStateChanged?.Invoke(fryingState);
                OnProgressChanged?.Invoke(0);
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
