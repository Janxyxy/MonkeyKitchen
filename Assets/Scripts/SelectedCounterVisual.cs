using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGO;

    private void Start()
    {
        baseCounter = GetComponentInParent<BaseCounter>();
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

        Show(false);
    }

    private void Player_OnSelectedCounterChanged(Player.OnSelectedCounterChangedEventArgs args)
    {

        Show(args.selectedCounter == baseCounter);

    }

    private void Show(bool v)
    {
        foreach (GameObject go in visualGO)
        {
            go.SetActive(v);
        }
    }
}
