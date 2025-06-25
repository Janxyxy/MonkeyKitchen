using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitinRecipeSOList;
    private float spawnTimer;
    private float spawnTimerMax = 5f;
    private int maxWaitingRecipes = 5;
    private int successfullRecipesDeliveredCount = 0;

    public static DeliveryManager Instance { get; private set; }

    public event Action OnRecipeDelivered;
    public event Action OnRecipeSpawned;

    public event Action OnRecipeSuccess;
    public event Action OnRecipeFailure;

    private void Awake()
    {
        waitinRecipeSOList = new List<RecipeSO>();
        spawnTimer = 0f;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTimerMax)
        {
            spawnTimer = 0f;
            AddRecipe();
        }
    }

    private void AddRecipe()
    {
        if (waitinRecipeSOList.Count < maxWaitingRecipes && recipeListSO.recipesList.Count > 0)
        {
            RecipeSO newRecipeSO = recipeListSO.recipesList[UnityEngine.Random.Range(0, recipeListSO.recipesList.Count)];
            waitinRecipeSOList.Add(newRecipeSO);

            OnRecipeSpawned?.Invoke();
        }
        else if (recipeListSO.recipesList.Count == 0)
        {
            Debug.LogWarning("Recipe list is empty. Cannot add a new recipe.");
        }
    }

    internal void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitinRecipeSOList.Count; i++)
        {
            RecipeSO recipeSO = waitinRecipeSOList[i];
            if (recipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetIngredientList().Count)
            {
                // Has the same number of ingredients
                bool allIngredientsMatch = true; 

                foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateIngredient in plateKitchenObject.GetIngredientList())
                    {
                        if (kitchenObjectSO == plateIngredient)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        allIngredientsMatch = false;
                        break;
                    }
                }

                if (allIngredientsMatch)
                {
                    // Recipe matches
                    successfullRecipesDeliveredCount++;
                    waitinRecipeSOList.RemoveAt(i);

                    OnRecipeDelivered?.Invoke();
                    OnRecipeSuccess?.Invoke();
                    return;
                }
            }
        }

        // No matching recipe found
        OnRecipeFailure?.Invoke();
        Debug.Log("No matching recipe found for delivery.");
    }

    internal IEnumerable<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitinRecipeSOList;
    }

    internal int GetSuccessfullRecipesDeliveredCount()
    {
        return successfullRecipesDeliveredCount;
    }
}
