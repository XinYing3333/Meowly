using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- 管理器與物件引用 ---
    public DialogueManager dialogueManager; // 引用對話管理器
    public GameObject gamePanel;
    public GameObject gameWinPanel;
    public GameObject startScene;


    public bool isWin;
    // --- 對話內容 ---
    // 可以在 Inspector 中直接輸入開場白
    [TextArea(3, 10)]
    public string[] introSentences;

    void Start()
    {
        // 遊戲一開始，先隱藏玩家和遊戲 UI
        if (startScene != null) startScene.SetActive(false);

        // 檢查 DialogueManager 是否存在
        if (dialogueManager != null)
        {
            
            dialogueManager.OnDialogueFinished += StartGame;

            // 讓對話管理器用我們設定好的句子開始對話
            //dialogueManager.StartDialogue(introSentences);
        }
        else
        {
            // 如果沒有設定對話管理器，直接開始遊戲
            Debug.LogWarning("Dialogue Manager not set. Starting game directly.");
            StartGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // 真正開始遊戲的邏輯
    void StartGame()
    {
        Debug.Log("Game Started!");

        // 在這裡可以安全地取消訂閱，避免重複觸發
        if (dialogueManager != null)
        {
            //dialogueManager.OnDialogueFinished -= StartGame;
        }

        if (!isWin)
        {
            startScene.SetActive(true);
            gamePanel.SetActive(false);
        }
        else
        {
            gameWinPanel.SetActive(true);
        }
        
    }

    void OnDestroy()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueFinished -= StartGame;
        }
    }
}   