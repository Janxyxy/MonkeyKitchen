using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGO;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGO.GetComponent<IHasProgress>();
        if(hasProgress == null)
        {
            Debug.LogError("IHasProgress component not found on hasProgressGO.");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f; // Initialize the progress bar to empty
        Show(false);
    }

    private void HasProgress_OnProgressChanged(float fill)
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
