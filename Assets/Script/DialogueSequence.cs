using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Game/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;                 // ���ܪ�
        [TextArea(2, 5)] public string content; // ��ܤ��e
        public bool darkenScene;               // �O�_�շt
        public bool brightenScene;             // �O�_�իG
    }

    public DialogueLine[] lines;
}
