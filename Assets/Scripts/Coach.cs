using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coach : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake(){
        allowCoach = false;
    }
    ActionOnE action;
    void Start()
    {
        action = GetComponent<ActionOnE>();
        action.action+=CoachUsed;
    }
    public static bool allowCoach = false;
    IEnumerator coachUsed(){
        PopUp.singleton.blackScreenOn();
        float saveSpeed = ActualComputerScreen.speed;
        ActualComputerScreen.speed = 100f;
        yield return new WaitForSeconds(1);
        ActualComputerScreen.speed = saveSpeed;
        PopUp.singleton.blackScreenOff();
        Character.AllowEverything();
        allowCoach = false;
    }
    public void CoachUsed(){
        if (!allowCoach && !RandomEventManager.eventAllowed)
            return;
        Character.CancelEverything(false);
        StartCoroutine(coachUsed());
    }
    // Update is called once per frame
    void Update()
    {
        if (!allowCoach && !RandomEventManager.eventAllowed){
            action.popUpText = "";
        }else{
            action.popUpText = "Нажми \"у\", чтобы вздремнуть на диване";
        }
    }
}
