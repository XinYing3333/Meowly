using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PuzzlePieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Reference")]
    [SerializeField] private GridBoardUI board;
    [SerializeField] private Canvas canvas;

    [Header("Footprint（相對格偏移）")]
    public List<Vector2Int> Footprint = new() { Vector2Int.zero };

    [Header("Visual (Optional)")]
    [SerializeField] private Image highlightWhileDragging;

    // 狀態
    private RectTransform rt;
    private Vector2 originalAnchoredPos;
    private Transform originalParent;
    private CanvasGroup cg;

    public  bool IsPlaced { get; private set; } = false;
    private Vector2Int placedAnchorCell = new(-1, -1);

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = gameObject.GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        if (board != null) board.RegisterPiece(this);
    }

    private void OnDisable()
    {
        if (board != null) board.UnregisterPiece(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalAnchoredPos = rt.anchoredPosition;
        originalParent = rt.parent;

        // 若已落位，先解除占用並通知
        if (IsPlaced)
        {
            board.UnplacePiece(this, placedAnchorCell);
            IsPlaced = false;
            board.NotifyPieceUnplaced(this);
            placedAnchorCell = new Vector2Int(-1, -1);
        }

        rt.SetAsLastSibling();
        cg.blocksRaycasts = false;
        if (highlightWhileDragging) highlightWhileDragging.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rt.parent, eventData.position, eventData.pressEventCamera, out localPos);
        rt.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (board != null && board.TryPlacePiece(this, eventData, out var anchorCell, out var snappedLocalPos))
        {
            rt.SetParent(board.transform, worldPositionStays: false);
            rt.anchoredPosition = snappedLocalPos;

            IsPlaced = true;
            placedAnchorCell = anchorCell;

            // ✅ 通知棋盤：這個拼圖成功落位
            board.NotifyPiecePlaced(this);
        }
        else
        {
            // 放回原位
            rt.SetParent(originalParent, worldPositionStays: false);
            rt.anchoredPosition = originalAnchoredPos;
        }

        cg.blocksRaycasts = true;
        if (highlightWhileDragging) highlightWhileDragging.enabled = false;
    }
}
