using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ScreenButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField]MeshRenderer meshRendererToChange;
    public bool isSell;
    bool isGray = false;
    [SerializeField] private Color gray,purple;
    // Update is called once per frame
    public bool firstUpdate = true;
    void Update()
    {
        bool newIsGray;
        if (isSell){
            newIsGray = ActualComputerScreen.currentlyBoughtBethovens == 0;
        }else{
            newIsGray = ActualComputerScreen.currentMoney < ActualComputerScreen.currentBethovenValue;
        }
        if (newIsGray != isGray || firstUpdate){
            meshRendererToChange.materials[0].color = newIsGray? gray:purple;
            isGray = newIsGray;
            firstUpdate = false;
        }
    }
    public Action onClick;
    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        if (!TradeManager.singleton.tradingStartedFlag)
            return;
        onClick?.Invoke();
    }
}
