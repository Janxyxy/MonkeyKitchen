using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipe", menuName = "Scriptable Objects/CuttingRecipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
