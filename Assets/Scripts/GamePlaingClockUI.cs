using UnityEngine;
using UnityEngine.UI;

public class GamePlaingClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;

    private void Start()
    {
        clockImage.fillAmount = 0f;
    }

    private void Update()
    {
        clockImage.fillAmount =  GameManager.Instance.GetGameplayTimerNormalized();
    }
}
