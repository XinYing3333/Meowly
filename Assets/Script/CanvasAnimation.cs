using UnityEngine;
using DG.Tweening;

public class CanvasAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1f;

    // �H�J
    public void FadeIn()
    {
        canvasGroup.DOFade(1f, fadeDuration)
                   .SetEase(Ease.InOutQuad);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // �H�X
    public void FadeOut()
    {
        canvasGroup.DOFade(0f, fadeDuration)
                   .SetEase(Ease.InOutQuad)
                   .OnComplete(() =>
                   {
                       // �H�X��T�Τ���
                       canvasGroup.interactable = false;
                       canvasGroup.blocksRaycasts = false;
                   });
    }
}