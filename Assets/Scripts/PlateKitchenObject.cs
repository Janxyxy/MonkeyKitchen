using System;
using System.Collections.Generic;
using UnityEngine;

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
     
        ingredientList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });

        return true;
    }

    internal List<KitchenObjectSO> GetIngredientList()
    {
        return ingredientList;
    }
}
