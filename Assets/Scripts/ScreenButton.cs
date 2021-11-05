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

    // Update is called once per frame
    void Update()
    {
        
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
