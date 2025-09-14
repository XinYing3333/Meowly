using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // 如果你要顯示勝利訊息的話

public class GameManager : MonoBehaviour
{
    public List<DraggableItem> allItems;
    public GameObject winPanel; // 拖入一個勝利UI Panel

    void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void CheckForWinCondition()
    {
        // 檢查是否所有物件都已經被放置
        foreach (var item in allItems)
        {
            if (!item.isPlaced)
            {
                // 只要有一個還沒放好，就直接返回
                return;
            }
        }

        // 如果迴圈跑完了，代表所有物件都放好了
        Debug.Log("遊戲勝利！");
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
}