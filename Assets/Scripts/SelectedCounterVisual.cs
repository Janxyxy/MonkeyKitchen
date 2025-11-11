using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGO;

    private void Start()
    {
        baseCounter = GetComponentInParent<BaseCounter>();

        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }

        Show(false);
    }

    private void Player_OnAnyPlayerSpawned()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
    }

    private void Player_OnSelectedCounterChanged(Player.OnSelectedCounterChangedEventArgs args)
    {

        Show(args.selectedCounter == baseCounter);

    }

    private void Show(bool v)
    {
        foreach (GameObject go in visualGO)
        {
            if (go != null)
                go.SetActive(v);
        }
    }
}
