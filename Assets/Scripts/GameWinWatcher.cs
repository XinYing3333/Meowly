using UnityEngine;

public class GameWinWatcher : MonoBehaviour
{
    [SerializeField] private GridBoardUI board;
    [SerializeField] private GameObject c2;
    [SerializeField] private GameManager gameManager;

    private void OnEnable()  { board.OnAllPiecesPlaced += HandleWin; }
    private void OnDisable() { board.OnAllPiecesPlaced -= HandleWin; }

    private void HandleWin()
    {
        c2.SetActive(true);
        gameManager.isWin = true;
    }
}