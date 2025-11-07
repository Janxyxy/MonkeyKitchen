using System;
using UnityEngine;
using Unity.Netcode;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;

    public event Action<float> OnProgressChanged;
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);

    private FryingRecipeSO currentFryingRecipeSO;
    private NetworkVariable<FryingState> fryingState = new NetworkVariable<FryingState>(FryingState.Idle);

    public event Action<FryingState> OnFryingStateChanged;

    public enum FryingState
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        fryingState.OnValueChanged += FryingState_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = currentFryingRecipeSO != null ? currentFryingRecipeSO.fryingProgressMax : 1f;

        OnProgressChanged?.Invoke(fryingTimer.Value / fryingTimerMax);
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = currentFryingRecipeSO != null ? currentFryingRecipeSO.fryingProgressMax : 1f;

        OnProgressChanged?.Invoke(burningTimer.Value / burningTimerMax);
    }

    private void FryingState_OnValueChanged(FryingState previousValue, FryingState newValue)
    {
        OnFryingStateChanged?.Invoke(fryingState.Value);

        if (fryingState.Value == FryingState.Idle || fryingState.Value == FryingState.Fried || fryingState.Value == FryingState.Burned)
        {
            OnProgressChanged?.Invoke(0f);
        }
    }

    private void Update()
    {
        if(!IsServer) return;

        if (HasKitchenObject())
        {
            switch (fryingState.Value)
            {
                case FryingState.Idle:
                    break;
                case FryingState.Frying:
                    fryingTimer.Value += Time.deltaTime;

                    if (fryingTimer.Value > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState.Value = FryingState.Fried;
                        burningTimer.Value = 0f;
                    }
                    break;
                case FryingState.Fried:
                    burningTimer.Value += Time.deltaTime;

                    if (burningTimer.Value > currentFryingRecipeSO.fryingProgressMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                        fryingState.Value = FryingState.Burned;    
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
                    KitchenObject kitchenObject = player.GetKitchenObject();

                    // Player has a KitchenObject and it matches a recipe input, so we can set the StoveCounter's KitchenObject.
                    kitchenObject.SetKitchenObjectParent(this);
                    currentFryingRecipeSO = GetFryingRecipeSoWithInput(kitchenObject);

                    InteractLogicPlaceObjectOnCounterServerRpc();
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        SetStateIdleServerRpc();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                SetStateIdleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        fryingState.Value = FryingState.Idle;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        fryingTimer.Value = 0f;
        fryingState.Value = FryingState.Frying;

        InteractLogicTakeObjectFromCounterClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void InteractLogicTakeObjectFromCounterClientRpc()
    {
        currentFryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject());
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
