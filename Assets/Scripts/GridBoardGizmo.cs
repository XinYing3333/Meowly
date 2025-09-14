#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// 在「非遊戲模式」與「遊戲模式」都能在 Scene 視窗可視化棋盤格。
/// 不改動你的 GridBoardUI，專責畫格線/外框/占用預覽。
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
public class GridBoardGizmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridBoardUI board;       // 指向你的棋盤（含寬高、cellSize、padding 的那支）
    [SerializeField] private RectTransform boardRect; // 通常就是 board 的 RectTransform

    [Header("Appearance")]
    [SerializeField] private Color borderColor = new Color(1f, 1f, 1f, 0.9f);
    [SerializeField] private Color gridColor   = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] private Color fillColor   = new Color(0.2f, 0.8f, 0.4f, 0.25f); // 占用格預覽（Play 時）
    [SerializeField] private float lineThickness = 2f;

    [Header("Options")]
    [SerializeField] private bool drawGridLines = true;
    [SerializeField] private bool drawBorder    = true;
    [SerializeField] private bool previewOccupiedWhenPlaying = true;

    private void Reset()
    {
        board = GetComponent<GridBoardUI>();
        boardRect = GetComponent<RectTransform>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (board == null || boardRect == null) return;

        var width    = GetPrivate<int>(board, "width");
        var height   = GetPrivate<int>(board, "height");
        var cellSize = GetPrivate<float>(board, "cellSize");
        var padding  = GetPrivate<Vector2>(board, "padding");

        // 以 RectTransform 的 local 為基準：左下角 = -size/2 + padding
        var size   = boardRect.rect.size;
        var origin = -size * 0.5f + padding;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

        // 畫外框
        if (drawBorder)
        {
            Handles.color = borderColor;
            var bl = boardRect.TransformPoint(new Vector3(origin.x, origin.y, 0));
            var br = boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, origin.y, 0));
            var tr = boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, origin.y + height * cellSize, 0));
            var tl = boardRect.TransformPoint(new Vector3(origin.x, origin.y + height * cellSize, 0));
#if UNITY_2020_1_OR_NEWER
            Handles.DrawAAPolyLine(lineThickness, new Vector3[] { bl, br, tr, tl, bl });
#else
            Handles.DrawPolyLine(new Vector3[] { bl, br, tr, tl, bl });
#endif
        }

        // 畫內部格線
        if (drawGridLines)
        {
            Handles.color = gridColor;

            // 垂直線
            for (int x = 1; x < width; x++)
            {
                float lx = origin.x + x * cellSize;
                var a = boardRect.TransformPoint(new Vector3(lx, origin.y, 0));
                var b = boardRect.TransformPoint(new Vector3(lx, origin.y + height * cellSize, 0));
#if UNITY_2020_1_OR_NEWER
                Handles.DrawAAPolyLine(lineThickness, a, b);
#else
                Handles.DrawLine(a, b);
#endif
            }

            // 水平線
            for (int y = 1; y < height; y++)
            {
                float ly = origin.y + y * cellSize;
                var a = boardRect.TransformPoint(new Vector3(origin.x, ly, 0));
                var b = boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, ly, 0));
#if UNITY_2020_1_OR_NEWER
                Handles.DrawAAPolyLine(lineThickness, a, b);
#else
                Handles.DrawLine(a, b);
#endif
            }
        }

        // 遊戲中：把已占用格子用半透明色塊標示（方便除錯）
        if (Application.isPlaying && previewOccupiedWhenPlaying)
        {
            var occ = GetPrivate<bool[,]>(board, "occupied");
            if (occ != null && occ.GetLength(0) == width && occ.GetLength(1) == height)
            {
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (!occ[x, y]) continue;
                    DrawFilledCell(origin, x, y, cellSize, fillColor);
                }
            }
        }
    }

    private void DrawFilledCell(Vector2 origin, int cx, int cy, float cell, Color c)
    {
        var p0 = boardRect.TransformPoint(new Vector3(origin.x +  cx      * cell, origin.y +  cy      * cell, 0));
        var p1 = boardRect.TransformPoint(new Vector3(origin.x + (cx + 1) * cell, origin.y +  cy      * cell, 0));
        var p2 = boardRect.TransformPoint(new Vector3(origin.x + (cx + 1) * cell, origin.y + (cy + 1) * cell, 0));
        var p3 = boardRect.TransformPoint(new Vector3(origin.x +  cx      * cell, origin.y + (cy + 1) * cell, 0));

        Handles.DrawSolidRectangleWithOutline(new Vector3[] { p0, p1, p2, p3 }, c, new Color(0,0,0,0));
    }

    // 讀 private 欄位（僅供可視化，不影響執行）
    private T GetPrivate<T>(object obj, string field)
    {
        var f = obj.GetType().GetField(field, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (f == null) return default;
        return (T)f.GetValue(obj);
    }
#endif
}
