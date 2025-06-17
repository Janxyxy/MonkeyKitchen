using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenObject", menuName = "ScriptableObjects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string objectName;
}
