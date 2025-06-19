using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> ingredientList;


    private void Awake()
    {
        ingredientList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
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
        return true;
    }
}
