using DG.Tweening;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public CanvasGroup dialogCanvas;
    public CanvasGroup commissionCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public  void ToCommission()
    {
        dialogCanvas.DOFade(0, 1).OnComplete(() => commissionCanvas.DOFade(1, 1));
    }
    
 

}
