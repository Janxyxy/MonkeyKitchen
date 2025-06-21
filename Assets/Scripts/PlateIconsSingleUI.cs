using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    internal void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {

            iconImage.gameObject.SetActive(true);
            iconImage.sprite = kitchenObjectSO.icon;
    }
}
