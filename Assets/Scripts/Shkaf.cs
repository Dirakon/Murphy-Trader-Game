using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shkaf : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake(){
        allowSkhaf = false;
    }
    ActionOnE action;
    void Start()
    {
        action = GetComponent<ActionOnE>();
        action.action+=ShkafUsed;
    }
    public static bool allowSkhaf = false;
    IEnumerator shkafUsed(){
        PopUp.singleton.blackScreenOn();
        yield return new WaitForSeconds(1);
        PopUp.singleton.blackScreenOff();
        Character.AllowEverything();
        allowSkhaf = false;
    }
    public void ShkafUsed(){
        if (!allowSkhaf)
            return;
        Character.CancelEverything(false);
        StartCoroutine(shkafUsed());
    }
    // Update is called once per frame
    void Update()
    {
        if (!allowSkhaf){
            action.popUpText = "";
        }else{
            action.popUpText = "Нажми \"у\", чтобы спрятаться в шкафу";
        }
    }
}

