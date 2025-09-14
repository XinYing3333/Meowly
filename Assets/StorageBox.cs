using UnityEngine;
using UnityEngine.EventSystems;

public class StorageBox : MonoBehaviour, IDropHandler
{
    public static int gridWidth = 3;
    public static int gridHeight = 3;
    public float cellSize = 100f; // 每個格子的像素大小，需要與你的UI設定匹配

    // 用一個二維陣列來記錄每個格子是否被佔用
    private bool[,] isOccupied = new bool[gridWidth, gridHeight];
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // 將世界座標 (滑鼠位置) 轉換為網格座標
    public Vector2Int WorldToGridPoint(Vector3 worldPosition)
    {
        // 先將世界座標轉換為相對於StorageBox左下角的本地座標
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, worldPosition, null, out localPoint);

        // 轉換為網格座標 (0,0) 到 (2,2)
        int x = Mathf.FloorToInt((localPoint.x + rectTransform.pivot.x * rectTransform.rect.width) / cellSize);
        int y = Mathf.FloorToInt((localPoint.y + rectTransform.pivot.y * rectTransform.rect.height) / cellSize);

        return new Vector2Int(x, y);
    }

    // 檢查某個區域是否可以放置物件
    public bool IsValidPlacement(Vector2Int gridPos, Vector2Int itemSize)
    {
        for (int x = 0; x < itemSize.x; x++)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                int checkX = gridPos.x + x;
                int checkY = gridPos.y + y;

                // 1. 檢查是否超出邊界
                if (checkX < 0 || checkX >= gridWidth || checkY < 0 || checkY >= gridHeight)
                {
                    return false;
                }

                // 2. 檢查是否與已放置的物件重疊
                if (isOccupied[checkX, checkY])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // 放置物件，並更新網格狀態
    public void PlaceObject(Vector2Int gridPos, Vector2Int itemSize)
    {
        for (int x = 0; x < itemSize.x; x++)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                isOccupied[gridPos.x + x, gridPos.y + y] = true;
            }
        }
    }

    // 計算物件在網格中對齊後的位置
    public Vector2 GetSnappedPosition(Vector2Int gridPos, Vector2Int itemSize)
    {
        // Now the function knows what 'itemSize' is because it's passed in as a parameter.
        float totalItemWidth = itemSize.x * cellSize;
        float totalItemHeight = itemSize.y * cellSize;

        float centerX = (gridPos.x * cellSize) + (totalItemWidth / 2f);
        float centerY = (gridPos.y * cellSize) + (totalItemHeight / 2f);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        return new Vector2(centerX - (parentWidth / 2f), centerY - (parentHeight / 2f));
    }
    
    // 雖然 DraggableItem 會處理放置邏輯，但保留 IDropHandler 可以方便未來擴充
    public void OnDrop(PointerEventData eventData)
    {
        // DraggableItem 會處理主要邏輯
    }
}