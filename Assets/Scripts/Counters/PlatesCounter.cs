using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float plateSpawnTimer;
    private float plateSpawnTime = 4f;

    private float platesSpawnedAmmount;
    private float platesSpawnedMaxAmmount = 4f;

    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;
        if (plateSpawnTimer >= plateSpawnTime)
        {
            plateSpawnTimer = 0f;
            SpawnPlate();
        }
    }

    internal override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmmount > 0)
            {
                platesSpawnedAmmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke();
            }
            else
            {
                Debug.Log("No plates available to pick up.");
            }
        }
        else
        {
            Debug.Log("Player already has a kitchen object.");
        }
    }

    private void SpawnPlate()
    {
        if (platesSpawnedAmmount < platesSpawnedMaxAmmount)
        {
            platesSpawnedAmmount++;

            OnPlateSpawned?.Invoke();
        }

    }
}
