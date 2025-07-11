using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        Updatevisual();
    }

    private void DeliveryManager_OnRecipeSpawned()
    {
        Updatevisual();
    }

    private void DeliveryManager_OnRecipeDelivered()
    {
        Updatevisual();
    }

    internal void Updatevisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate)
                continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;
            DeliveryManager.Instance.OnRecipeSpawned -= DeliveryManager_OnRecipeSpawned;
        }
    }
}
