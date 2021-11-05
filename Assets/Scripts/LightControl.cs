using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    private static LightControl singleton;
    [SerializeField]
    private Light[] ceilingLights;
    [SerializeField]
    private Light computerLight;
    [SerializeField]
    private Light rubLight;
    private static Color colorBlue = Color.blue, colorRed = Color.red;
    // Start is called before the first frame update
    void Awake(){
        singleton=this;
    }
    public static bool electricityState= true;
    public static void SetPCLight(bool state){
        singleton.computerLight.color = state?colorBlue:colorRed;
    }
    public static void SetAllLights(bool state){
        electricityState=state;
        foreach (var light in singleton.ceilingLights)
        {
            light.gameObject.SetActive(state);
        }
        if (!state){
            PC.ShutDown();
        }
        singleton.computerLight.gameObject.SetActive(state);
        singleton.rubLight.color = state?colorBlue:colorRed;
    }

    void Start()
    {
        SetAllLights(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
