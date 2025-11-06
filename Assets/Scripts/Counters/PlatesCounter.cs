using System;
using UnityEngine;
using Unity.Netcode;

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
        if (!IsServer)
        {
            return;
        }

        if (!GameManager.Instance.IsGamePlaing())
        {
            return;
        }

        plateSpawnTimer += Time.deltaTime;
        if (plateSpawnTimer >= plateSpawnTime)
        {
            plateSpawnTimer = 0f;
            SpawnPlateServerRPC();
        }
    }


    // ServerRPC není potøeba protože bìží pouze na serveru, ale code monkey to tak má tak co už
    [ServerRpc]
    private void SpawnPlateServerRPC()
    {
        SpawnPlateClientRPC();
    }

    [ClientRpc]
    private void SpawnPlateClientRPC()
    {
        SpawnPlate();
    }


    internal override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                InteractLogicServerRpc();
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


    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        platesSpawnedAmmount--;
        OnPlateRemoved?.Invoke();
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
