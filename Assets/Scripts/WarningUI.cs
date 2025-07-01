using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using VInspector;

public class WarningUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningTMP;

    private int endX = 720;
    private int startX = 1275;
    private float moveDuration = 0.2f;

    private Tween currentTween;

    [Button]
    public void ShowWarning(string warningText, float showDuration)
    {
        warningTMP.text = warningText;

        // Zruš pøípadný starý tween
        currentTween?.Kill();

        // Ujisti se, že zaèínáme schovaní
        transform.localPosition = new Vector3(startX, transform.localPosition.y, 0);

        // Vytvoø sekvenci: pøijeï – poèkej – odjeï
        currentTween = DOTween.Sequence()
          .SetUpdate(true) // ignoruje Time.timeScale
          .Append(transform.DOLocalMoveX(endX, moveDuration).SetEase(Ease.InOutSine))
          .AppendInterval(showDuration)
          .Append(transform.DOLocalMoveX(startX, moveDuration).SetEase(Ease.InOutSine))
          .OnComplete(() => currentTween = null);
    }

}
