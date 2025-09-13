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

    private bool cannotPlay = true;

    void Awake()
    {
        emoteAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    
    void Update()
    {
        if(cannotPlay)return;
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator); 
        }
        else
        {
            //keyE.SetActive(false);
        }
        
    }

    public void StartDialogue()
    {
        cannotPlay = false;
    }
}