using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> ingredientList;

    public event Action<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    protected override void Awake()
    {
        base.Awake();
        ingredientList = new List<KitchenObjectSO>();
    }

    internal bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        if (ingredientList.Contains(kitchenObjectSO))
        {
            return false;
        }

        AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));

        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);


        ingredientList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });

    }

    internal List<KitchenObjectSO> GetIngredientList()
    {
        return ingredientList;
    }
}
