using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItemController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // --- 新增部分 ---
    // 設定這個物品對應的正確插槽 ID
    public int correctSlotID;

    // --- 以下變數維持不變 ---
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private Transform originalParent;
    private Canvas canvas;
    public bool isDroppedSuccessfully = false;

    // Start 函式維持不變
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        originalScale = transform.localScale;
    }

    // OnPointerDown 函式維持不變 (我們還是用縮小的效果)
    public void OnPointerDown(PointerEventData eventData)
    {
        isDroppedSuccessfully = false;
        originalPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        transform.localScale = originalScale * 0.8f;
    }

    // OnDrag 函式維持不變
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // OnPointerUp 函式維持不變，它的邏輯非常通用，剛好符合我們現在的需求！
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if (!isDroppedSuccessfully)
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }
}