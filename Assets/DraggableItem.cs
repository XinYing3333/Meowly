using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("物件尺寸 (單位：格子)")]
    public Vector2Int size = new Vector2Int(1, 1);

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Transform startParent;
    private StorageBox storageBox;
    private GameManager gameManager;
    
    public bool isPlaced { get; private set; } = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        storageBox = FindObjectOfType<StorageBox>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return; // 如果已經放好了，就不ให้拖動

        startPosition = rectTransform.position;
        startParent = transform.parent;
        
        // 提高渲染層級，並讓滑鼠事件可以穿透
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (isPlaced) return;

        // 1. 取得物件中心點所在的網格座標
        Vector2Int centerGridPos = storageBox.WorldToGridPoint(rectTransform.position);
        Debug.Log("滑鼠放開時的網格座標 (Grid Position): " + centerGridPos);

        // 2. ⭐【關鍵修正】⭐ 從中心點座標計算出物件的左下角座標
        //    我們利用整數除法來計算偏移量。例如 2/2=1, 3/2=1。
        Vector2Int offset = new Vector2Int(size.x / 2, size.y / 2);
        Vector2Int bottomLeftGridPos = centerGridPos - offset;

        // 3. 使用修正後的「左下角座標」來進行放置判斷和位置計算
        if (storageBox.IsValidPlacement(bottomLeftGridPos, size))
        {
            // 使用左下角座標來計算最終要吸附到的中心位置
            Vector2 snappedPos = storageBox.GetSnappedPosition(bottomLeftGridPos, size);
            Debug.Log("準備吸附到的本地座標 (Snapped Local Position): " + snappedPos);

            // 更新網格狀態 (傳入左下角座標)
            storageBox.PlaceObject(bottomLeftGridPos, size);

            // 設定父物件與位置
            rectTransform.SetParent(storageBox.transform);
            rectTransform.localPosition = snappedPos;
        
            isPlaced = true;
            gameManager.CheckForWinCondition();
        }
        else
        {
            // 如果放置無效，彈回原位
            rectTransform.position = startPosition;
        }
    }
}