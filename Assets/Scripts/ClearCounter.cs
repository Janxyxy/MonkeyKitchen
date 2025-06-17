using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    internal void Interact()
    {
        Debug.Log("Interacting!");

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint.position, Quaternion.identity);
        kitchenObjectTransform.SetParent(counterTopPoint);
        //kitchenObjectTransform.localPosition = Vector3.zero; 
    }
}
