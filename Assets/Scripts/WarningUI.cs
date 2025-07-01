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

        // Zru� p��padn� star� tween
        currentTween?.Kill();

        // Ujisti se, �e za��n�me schovan�
        transform.localPosition = new Vector3(startX, transform.localPosition.y, 0);

        // Vytvo� sekvenci: p�ije� � po�kej � odje�
        currentTween = DOTween.Sequence()
          .SetUpdate(true) // ignoruje Time.timeScale
          .Append(transform.DOLocalMoveX(endX, moveDuration).SetEase(Ease.InOutSine))
          .AppendInterval(showDuration)
          .Append(transform.DOLocalMoveX(startX, moveDuration).SetEase(Ease.InOutSine))
          .OnComplete(() => currentTween = null);
    }

}
