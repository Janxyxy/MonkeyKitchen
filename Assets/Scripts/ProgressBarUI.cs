using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += HandleCuttingProgressChanged;
        barImage.fillAmount = 0f; // Initialize the progress bar to empty
        Show(false);
    }

    private void HandleCuttingProgressChanged(float fill)
    {
       barImage.fillAmount = fill;

        if(fill == 0 || fill == 1)
        {
            Show(false);
        }
        else
        {
            Show(true);
        }
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }
}
