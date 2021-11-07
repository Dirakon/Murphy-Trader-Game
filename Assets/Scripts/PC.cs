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
        pcState = true;
        singleton = this;
        currentText = "Нажми \"у\", чтобы выключить ПК";
    }
    void Start()
    {
        action = GetComponentInChildren<ActionOnE>();
        action.action += ClickOnPC;
    }
    public void ClickOnPC()
    {
        if (pcState)
        {
            ShutDown();
        }
        else if (LightControl.electricityState)
        {
            RandomEventManager.RollUpdate();
            TryToTurnOn();
        }
    }
    public static void ShutDown()
    {
        pcState = false;
        singleton.currentText = "Нажми \"у\", чтобы включить ПК";
        LightControl.SetPCLight(false);
        singleton.materialToChange.materials[2].color = Color.red;
    }
    string currentText;
    public static void TryToTurnOn()
    {
        if (LightControl.electricityState)
        {
            singleton.currentText = "Нажми \"у\", чтобы выключить ПК";
            pcState = true;
            LightControl.SetPCLight(true);
            singleton.materialToChange.materials[2].color = Color.blue;
        }
    }
    // Update is called once per frame
    void Update()
    {
        action.popUpText = LightControl.electricityState ? currentText : "Сначала включи электричество!";
    }
}
