using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Game/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;                 // 說話者
        [TextArea(2, 5)] public string content; // 對話內容
        public bool darkenScene;               // 是否調暗
        public bool brightenScene;             // 是否調亮
    }

    public DialogueLine[] lines;
}
