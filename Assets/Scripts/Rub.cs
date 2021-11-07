using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rub : MonoBehaviour
{
    // Start is called before the first frame update
    public ActionOnE action;
    public MeshRenderer thingToChange;
        public GameObject rubRotator;
    void Awake(){
        singleton=this;
    }
    private static Rub singleton;
    void Start()
    {
        
        action = GetComponent<ActionOnE>();
        action.action += ClickOnRub;
    }
    public static void ClickOnRub(){
        if (LightControl.electricityState){
            LightControl.SetAllLights(false);
            singleton.action.popUpText=singleton.action.popUpText.Replace("выключить","включить")
            .Replace("off","on");
            singleton.rubRotator.transform.localRotation = 
            Quaternion.Euler(-25,0,0);
            singleton.thingToChange.materials[singleton.thingToChange.materials.Length-1].color = Color.red;
        }else{
            LightControl.SetAllLights(true);
            singleton.action.popUpText=singleton.action.popUpText.Replace("включить","выключить")
            .Replace("on","off");
            singleton.rubRotator.transform.localRotation = 
            Quaternion.Euler(25,0,0);
            singleton.thingToChange.materials[singleton.thingToChange.materials.Length-1].color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
