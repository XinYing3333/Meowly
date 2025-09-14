    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
using DG.Tweening;

public class DialogScript : MonoBehaviour
{
        public DialogueSequence dialogueAsset;
        private int index;
        public TMP_Text owner;
        public TMP_Text detail;
        public GameObject dialog;
    public CanvasGroup canvasGroup;
    public Image CatImage;
    public GameObject panel;
    public float typingSpeed = 0.05f;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {
       
        if (dialogueAsset == null || dialogueAsset.lines.Length == 0)
            {
                Debug.LogError("No Data");
                return;
            }
        canvasGroup.DOFade(1, 1).OnComplete(()=> { changeDialog(); });
         
           
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                changeDialog();
            }

        }
    

    public void SetDialogue(DialogueSequence newDialogue)
    {
        dialogueAsset = newDialogue;
        index = 0;
        dialog.SetActive(true);
        canvasGroup.alpha = 1; 
        changeDialog();
    }
    public void openCommision()
    {
        
       
    }

    private void closeDialog()
        {
        canvasGroup.DOFade(0, 1).OnComplete(() => dialog.SetActive(false));


    }
        private void changeDialog()
        {
            if (index >= dialogueAsset.lines.Length )
            {
                closeDialog();
                return;
            }
      
            owner.text = dialogueAsset.lines[index].speaker;
        if (dialogueAsset.lines[index].brightenScene)
        {
           CatImage.DOColor(Color.white, 0.5f);
        }
        else
        {

            CatImage.DOColor(Color.gray, 0.5f);
        }
            StartCoroutine(TypeSentence(dialogueAsset.lines[index].content));
            index++;
        }
 

        IEnumerator TypeSentence(string sentence)
        {
            detail.text = ""; // ¥ý²MªÅ
            foreach (char letter in sentence)
            {
                detail.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }


        public void onMouseClick()
        {
            changeDialog();

        }
    }
