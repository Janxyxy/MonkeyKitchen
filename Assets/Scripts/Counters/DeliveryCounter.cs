using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("Multiple DeliveryCounter instances detected. Destroying duplicate.");
            return;
        }

        Instance = this;
    }
    internal override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // Only accept a plate
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }       
        }
    }
}
