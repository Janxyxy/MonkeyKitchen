using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using VInspector;


public class DeliveryManager : NetworkBehaviour { 
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitinRecipeSOList;
    private float spawnTimer;
    private float spawnTimerMax = 5f;
    private int maxWaitingRecipes = 5;
    [SerializeField, VInspector.ReadOnly] private int successfullRecipesDeliveredCount = 0;

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
        if (!IsServer)
        {
            return;
        }

        if (!GameManager.Instance.IsGamePlaing())
        {
            return;
        }

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
            int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipesList.Count);

            SpawnNewWaitingRecipieClientRpc(waitingRecipeSOIndex);
          
        }
        else if (recipeListSO.recipesList.Count == 0)
        {
            Debug.LogWarning("Recipe list is empty. Cannot add a new recipe.");
        }
    }


    [ClientRpc]
    private void SpawnNewWaitingRecipieClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO newRecipeSO = recipeListSO.recipesList[waitingRecipeSOIndex];
        waitinRecipeSOList.Add(newRecipeSO);
        OnRecipeSpawned?.Invoke();
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
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }

        // No matching recipe found
        DeliverIncorectRecipeServerRpc();
        Debug.Log("No matching recipe found for delivery.");
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorectRecipeServerRpc()
    {
        DeliverIncorectRecipeClientRPC();
    }

    [ClientRpc]
    private void DeliverIncorectRecipeClientRPC()
    {
        OnRecipeFailure?.Invoke();
    }

    [ServerRpc(RequireOwnership =false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOListIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        successfullRecipesDeliveredCount++;
        waitinRecipeSOList.RemoveAt(waitingRecipeSOListIndex);

        OnRecipeDelivered?.Invoke();
        OnRecipeSuccess?.Invoke();
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
