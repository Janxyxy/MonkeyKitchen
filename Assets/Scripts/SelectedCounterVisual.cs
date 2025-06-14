using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGO;

    private void Start()
    {
        clearCounter = GetComponentInParent<ClearCounter>();
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(Player.OnSelectedCounterChangedEventArgs args)
    {

        Show(args.selectedCounter == clearCounter);

    }

    private void Show(bool v)
    {
        visualGO.SetActive(v);
    }
}
