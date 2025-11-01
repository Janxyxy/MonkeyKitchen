using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KitchenObjectsListSO", menuName = "Scriptable Objects/KitchenObjectsListSO")]
public class KitchenObjectsListSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList;
}
