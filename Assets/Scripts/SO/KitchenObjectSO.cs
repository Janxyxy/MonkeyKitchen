using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenObject", menuName = "Scriptable Objects/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string objectName;
}
