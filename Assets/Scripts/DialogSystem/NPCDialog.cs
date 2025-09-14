using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [Header("KeyE")]
    [SerializeField] private GameObject keyE;
    
    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;
    
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool cannotPlay = false;

    void Awake()
    {
    }
    
    void Update()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying && !cannotPlay)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator);
            cannotPlay = true;
        }
    }
}