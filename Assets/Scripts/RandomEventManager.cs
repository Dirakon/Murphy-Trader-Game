using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static bool LightsGoOff(){
        if (!LightControl.electricityState)
            return false;

        Rub.ClickOnRub();

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
