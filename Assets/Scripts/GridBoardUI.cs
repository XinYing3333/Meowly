using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class GridBoardUI : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private int width  = 9;
    [SerializeField] private int height = 9;
    [SerializeField] private float cellSize = 64f;
    [SerializeField] private Vector2 padding = new Vector2(8, 8);

    // 舊：棋盤全滿事件（保留相容，但不再當勝利條件）
    public event Action OnBoardFilled;

    // 新：所有拼圖都被放上事件（請改訂閱這個）
    public event Action OnAllPiecesPlaced;

    // ---- 勝利統計（新的勝利條件）----
    private readonly HashSet<PuzzlePieceUI> _registeredPieces = new();
    private int _placedCount = 0;     // 目前已成功落位的拼圖數

    private bool[,] occupied;
    private RectTransform boardRect;

    private void Awake()
    {
        boardRect = GetComponent<RectTransform>();
        if (occupied == null || occupied.GetLength(0) != width || occupied.GetLength(1) != height)
            occupied = new bool[width, height];
    }

    private void OnValidate()
    {
        if (width < 1) width = 1;
        if (height < 1) height = 1;
        if (cellSize < 1f) cellSize = 1f;

        if (occupied == null || occupied.GetLength(0) != width || occupied.GetLength(1) != height)
            occupied = new bool[width, height];

#if UNITY_EDITOR
        if (!Application.isPlaying) SceneView.RepaintAll();
#endif
    }

    // ====== 供 PuzzlePieceUI 註冊/通知 ======
    public void RegisterPiece(PuzzlePieceUI piece)
    {
        if (_registeredPieces.Add(piece))
        {
            if (piece.IsPlaced) _placedCount++;
            CheckWinByPieces();
        }
    }

    public void UnregisterPiece(PuzzlePieceUI piece)
    {
        if (_registeredPieces.Remove(piece))
        {
            if (piece.IsPlaced) _placedCount = Mathf.Max(0, _placedCount - 1);
            CheckWinByPieces(); // 一般不會在這裡勝利，但保持一致性
        }
    }

    internal void NotifyPiecePlaced(PuzzlePieceUI piece)
    {
        if (_registeredPieces.Contains(piece))
        {
            _placedCount = Mathf.Clamp(_placedCount + 1, 0, _registeredPieces.Count);
            CheckWinByPieces();
        }
    }

    internal void NotifyPieceUnplaced(PuzzlePieceUI piece)
    {
        if (_registeredPieces.Contains(piece))
        {
            _placedCount = Mathf.Max(0, _placedCount - 1);
            // 取消放置當然不會勝利，不呼叫事件，只更新統計
        }
    }

    private void CheckWinByPieces()
    {
        if (_registeredPieces.Count > 0 && _placedCount == _registeredPieces.Count)
        {
            // ✅ 新的勝利條件：所有關卡內拼圖都已落位
            OnAllPiecesPlaced?.Invoke();
        }
    }

    // === 對外 API（與你原本一樣） ===
    public bool TryPlacePiece(PuzzlePieceUI piece, PointerEventData endDragEventData, out Vector2Int anchorCell, out Vector2 snappedLocalPos)
    {
        if (!ScreenToLocalOnBoard(endDragEventData.position, out var localPos))
        {
            anchorCell = default;
            snappedLocalPos = default;
            return false;
        }

        anchorCell = LocalToCellFloor(localPos);

        if (!CanPlaceAt(piece.Footprint, anchorCell))
        {
            snappedLocalPos = default;
            return false;
        }

        MarkOccupy(piece.Footprint, anchorCell, true);
        snappedLocalPos = CellToLocalCenter(anchorCell);

        // 舊的「棋盤全滿」判斷（保留，但不當勝利條件）
        if (IsBoardFull()) OnBoardFilled?.Invoke();

        return true;
    }

    public void UnplacePiece(PuzzlePieceUI piece, Vector2Int anchorCell)
    {
        if (anchorCell.x < 0) return;
        MarkOccupy(piece.Footprint, anchorCell, false);
    }

    // === 內部工具 ===
    private bool ScreenToLocalOnBoard(Vector2 screenPos, out Vector2 local)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(boardRect, screenPos, null, out local);
    }

    private Vector2Int LocalToCellFloor(Vector2 local)
    {
        Vector2 origin = -boardRect.rect.size * 0.5f + padding;
        Vector2 delta = local - origin;
        int x = Mathf.FloorToInt(delta.x / cellSize);
        int y = Mathf.FloorToInt(delta.y / cellSize);
        return new Vector2Int(x, y);
    }

    private Vector2 CellToLocalCenter(Vector2Int cell)
    {
        Vector2 origin = -boardRect.rect.size * 0.5f + padding;
        float x = origin.x + (cell.x + 0.5f) * cellSize;
        float y = origin.y + (cell.y + 0.5f) * cellSize;
        return new Vector2(x, y);
    }

    private bool InBounds(Vector2Int c) => c.x >= 0 && c.x < width && c.y >= 0 && c.y < height;

    private bool CanPlaceAt(List<Vector2Int> footprint, Vector2Int anchorCell)
    {
        foreach (var o in footprint)
        {
            var c = anchorCell + o;
            if (!InBounds(c)) return false;
            if (occupied[c.x, c.y]) return false;
        }
        return true;
    }

    private void MarkOccupy(List<Vector2Int> footprint, Vector2Int anchorCell, bool value)
    {
        foreach (var o in footprint)
        {
            var c = anchorCell + o;
            if (InBounds(c)) occupied[c.x, c.y] = value;
        }
#if UNITY_EDITOR
        if (!Application.isPlaying) SceneView.RepaintAll();
#endif
    }

    private bool IsBoardFull()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (!occupied[x, y]) return false;
        return true;
    }

#if UNITY_EDITOR
    // ===== 仍保留你之前的可視化（節錄）=====
    private void OnDrawGizmos()
    {
        if (boardRect == null) boardRect = GetComponent<RectTransform>();
        if (boardRect == null) return;

        var size   = boardRect.rect.size;
        var origin = -size * 0.5f + padding;

        // 外框
        Handles.color = new Color(1f, 1f, 1f, 0.9f);
        var bl = boardRect.TransformPoint(new Vector3(origin.x, origin.y, 0));
        var br = boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, origin.y, 0));
        var tr = boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, origin.y + height * cellSize, 0));
        var tl = boardRect.TransformPoint(new Vector3(origin.x, origin.y + height * cellSize, 0));
        Handles.DrawAAPolyLine(2f, new Vector3[] { bl, br, tr, tl, bl });

        // 格線
        Handles.color = new Color(1f, 1f, 1f, 0.35f);
        for (int x = 1; x < width; x++)
        {
            float lx = origin.x + x * cellSize;
            Handles.DrawAAPolyLine(1.5f,
                boardRect.TransformPoint(new Vector3(lx, origin.y, 0)),
                boardRect.TransformPoint(new Vector3(lx, origin.y + height * cellSize, 0)));
        }
        for (int y = 1; y < height; y++)
        {
            float ly = origin.y + y * cellSize;
            Handles.DrawAAPolyLine(1.5f,
                boardRect.TransformPoint(new Vector3(origin.x, ly, 0)),
                boardRect.TransformPoint(new Vector3(origin.x + width * cellSize, ly, 0)));
        }
    }
#endif
}
