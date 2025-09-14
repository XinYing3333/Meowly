using UnityEngine;

public class GameWinWatcher : MonoBehaviour
{
    [SerializeField] private GridBoardUI board;

    private void OnEnable()  { board.OnAllPiecesPlaced += HandleWin; }
    private void OnDisable() { board.OnAllPiecesPlaced -= HandleWin; }

    private void HandleWin()
    {
        Debug.Log("🎉 全部拼圖都放上去了，勝利！");
        // TODO: 顯示勝利 UI / 結算 / 下一關
    }
}