using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> plateVisualgameObjectList;

    private void Awake()
    {
        plateVisualgameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved()
    {
        GameObject plateGO = plateVisualgameObjectList[plateVisualgameObjectList.Count - 1];
        plateVisualgameObjectList.Remove(plateGO);
        Destroy(plateGO);
    }

    private void PlatesCounter_OnPlateSpawned()
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateVisualgameObjectList.Count * plateOffsetY, 0);

        plateVisualgameObjectList.Add(plateVisualTransform.gameObject);
    }
}
