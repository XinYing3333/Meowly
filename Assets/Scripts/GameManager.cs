using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- 管理器與物件引用 ---
    public DialogueManager dialogueManager; // 引用對話管理器
    public GameObject player;
    public GameObject gameUI;

    // --- 對話內容 ---
    // 可以在 Inspector 中直接輸入開場白
    [TextArea(3, 10)]
    public string[] introSentences;

    void Start()
    {
        // 遊戲一開始，先隱藏玩家和遊戲 UI
        if (player != null) player.SetActive(false);
        if (gameUI != null) gameUI.SetActive(false);

        // 檢查 DialogueManager 是否存在
        if (dialogueManager != null)
        {
            // 訂閱 DialogueManager 的對話結束事件
            // 當 OnDialogueFinished 事件觸發時，執行 StartGame 函式
            dialogueManager.OnDialogueFinished += StartGame;

            // 讓對話管理器用我們設定好的句子開始對話
            dialogueManager.StartDialogue(introSentences);
        }
        else
        {
            // 如果沒有設定對話管理器，直接開始遊戲
            Debug.LogWarning("Dialogue Manager not set. Starting game directly.");
            StartGame();
        }
    }

    // 真正開始遊戲的邏輯
    void StartGame()
    {
        Debug.Log("Game Started!");

        // 在這裡可以安全地取消訂閱，避免重複觸發
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueFinished -= StartGame;
        }

        // 顯示玩家和遊戲 UI
        if (player != null) player.SetActive(true);
        if (gameUI != null) gameUI.SetActive(true);

        // 在這裡可以觸發其他遊戲腳本的開始函式
    }

    // 確保在物件被銷毀時也取消訂閱，避免記憶體洩漏
    void OnDestroy()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueFinished -= StartGame;
        }
    }
}   