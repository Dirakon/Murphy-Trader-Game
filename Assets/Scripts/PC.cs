using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    private static PC singleton;
    public MeshRenderer materialToChange;
    public static bool pcState = true;
    public ActionOnE action;
    // Start is called before the first frame update
    void Awake()
    {
        singleton=this;
    }
    void Start(){
        action = GetComponentInChildren<ActionOnE>();
        action.action += ClickOnPC;
    }
    public void ClickOnPC(){
        if (pcState){
            ShutDown();
        }else if (LightControl.electricityState){
            TryToTurnOn();
        }
    }
    public static void ShutDown(){
        pcState = false;
            singleton.action.popUpText=singleton.action.popUpText.Replace("выключить","включить")
            .Replace("off","on");
        LightControl.SetPCLight(false);
            singleton.materialToChange.materials[2].color = Color.red;
    }
    public static void TryToTurnOn(){
        if (LightControl.electricityState){
            singleton.action.popUpText=singleton.action.popUpText.Replace("включить","выключить")
            .Replace("on","off");
            pcState = true;
            LightControl.SetPCLight(true);
            singleton.materialToChange.materials[2].color = Color.blue;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
