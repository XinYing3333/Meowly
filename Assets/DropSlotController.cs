using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlotController : MonoBehaviour, IDropHandler
{
    public int slotID; 

    public void OnDrop(PointerEventData eventData)
    {
        // ======================= 超級偵錯開始 =======================
        
        Debug.Log("==================================================");
        Debug.Log("偵測到 Drop 事件！");
        
        // 檢查是哪個插槽接收到事件
        Debug.Log("接收事件的插槽是: " + gameObject.name + " (ID: " + this.slotID + ")");

        // 檢查有沒有抓到被拖曳的物件
        if (eventData.pointerDrag == null)
        {
            Debug.LogError("錯誤：抓不到被拖曳的物件 (pointerDrag is null)!");
            return;
        }

        GameObject droppedItemObject = eventData.pointerDrag;
        Debug.Log("被放下的物件是: " + droppedItemObject.name);

        // 檢查物件身上有沒有掛 DraggableItemController 腳本
        DraggableItemController draggableItem = droppedItemObject.GetComponent<DraggableItemController>();
        if (draggableItem == null)
        {
            Debug.LogError("錯誤：被放下的物件 " + droppedItemObject.name + " 身上沒有 DraggableItemController 腳本!");
            return;
        }

        // 這是最關鍵的一步：印出準備比對的兩個 ID
        Debug.LogWarning("--- 核心比對 --- >> 物品ID [" + draggableItem.correctSlotID + "] vs 插槽ID [" + this.slotID + "]");

        // 核心邏輯：比對 ID
        if (draggableItem.correctSlotID == this.slotID)
        {
            Debug.Log("<color=green>結果：ID 比對成功！執行吸附鎖定。</color>");

            draggableItem.isDroppedSuccessfully = true;
            droppedItemObject.transform.position = this.transform.position;
            draggableItem.enabled = false; 
        }
        else
        {
            Debug.Log("<color=red>結果：ID 比對失敗！物品將彈回。</color>");
        }
        
        Debug.Log("==================================================");

        // ======================= 超級偵錯結束 =======================
    }
}