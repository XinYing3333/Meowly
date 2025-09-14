using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    [SerializeField]private Animator animator;
    [SerializeField]private GameObject gamePanel;
    [SerializeField]private bool start;


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
        StartCoroutine(OpenGame());
    }

    IEnumerator OpenGame()
    {
        if(start)yield break;
        yield return new WaitForSeconds(1.5f);
        gamePanel.SetActive(true);
        start = true;
    }
}