using UnityEngine;
using DG.Tweening;

public class CanvasAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1f;

    // 淡入
    public void FadeIn()
    {
        canvasGroup.DOFade(1f, fadeDuration)
                   .SetEase(Ease.InOutQuad);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // 淡出
    public void FadeOut()
    {
        canvasGroup.DOFade(0f, fadeDuration)
                   .SetEase(Ease.InOutQuad)
                   .OnComplete(() =>
                   {
                       // 淡出後禁用互動
                       canvasGroup.interactable = false;
                       canvasGroup.blocksRaycasts = false;
                   });
    }
}