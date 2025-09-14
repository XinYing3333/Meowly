using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    [SerializeField]private Animator animator;


    private void Update()
    {
        if (!animator.GetBool("select")) return;
        if (Input.GetMouseButton(0))
        {
            StartGame();
        }
    }

    // 滑鼠移到 UI 元素上
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("select", true);
    }

    // 滑鼠離開 UI 元素
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("select", false);
    }

    public void StartGame()
    {
        animator.SetTrigger("start");
    }
}