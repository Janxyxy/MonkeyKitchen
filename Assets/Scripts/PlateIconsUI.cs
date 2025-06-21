using System;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

    }

    private void PlateKitchenObject_OnIngredientAdded(PlateKitchenObject.OnIngredientAddedEventArgs args)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in this.transform)
        {
            if (child == iconTemplate)
                continue; // Skip the template itself

            Destroy(child.gameObject); // Remove existing icons
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetIngredientList())
        {
            Transform iconTranform = Instantiate(iconTemplate, this.transform);
            iconTranform.gameObject.SetActive(true);
            iconTranform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
