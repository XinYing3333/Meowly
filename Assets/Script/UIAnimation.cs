using UnityEngine;
using DG.Tweening;
public class UIAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup.DOFade(1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
