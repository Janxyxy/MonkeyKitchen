using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Scriptable Objects/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> kitchenObjectSOList;
}
