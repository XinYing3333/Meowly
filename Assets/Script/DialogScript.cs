    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
   
public class DialogScript : MonoBehaviour
{
        public DialogueSequence dialogueAsset;
        private int index;
        public TMP_Text owner;
        public TMP_Text detail;
        public GameObject dialog;

        public float typingSpeed = 0.05f;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {
            if (dialogueAsset == null || dialogueAsset.lines.Length == 0)
            {
                Debug.LogError("No Data");
                return;
            }
           
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                changeDialog();
            }

        }

        private void closeDialog()
        {

        dialog.SetActive(false);

        }
        private void changeDialog()
        {
            if (index >= dialogueAsset.lines.Length - 1)
            {
                closeDialog();
                return;
            }
      
            owner.text = dialogueAsset.lines[index].speaker;
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
