using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public ActionOnE action;
    void Awake(){
        singleton=this;
    }
    void Start()
    {
        action = GetComponent<ActionOnE>();
        action.action += GetIntoTrading;
    }
    public void GetIntoTrading(){
        if (!LightControl.electricityState)
            return;
        Character.CancelEverything();
        StartCoroutine(intoTradingSmooth());
    }
    public float speed = 5f;
    public static TradeManager singleton;
    Vector3 saveCamPosition;
    Quaternion saveQuaternion;
    public Vector3 offset = new Vector3(0,0,2);
    IEnumerator intoTradingSmooth(){
        Camera camera = Camera.main;
        saveQuaternion = camera.transform.rotation;
        saveCamPosition = camera.transform.position;
        Quaternion endQuaternion = Quaternion.Euler(0,180,0);
        Vector3 endPosition = transform.position + offset;
        for(float t = 0; t < 1; t += Time.deltaTime*speed){
            camera.transform.position = Vector3.Lerp(saveCamPosition,endPosition,t);
            camera.transform.rotation = Quaternion.Lerp(saveQuaternion,endQuaternion,t);
            yield return null;
        }
        tradingStartedFlag = true;
    }
    IEnumerator outOfTradingSmooth(){
        Camera camera = Camera.main;
        Vector3 startPosition = camera.transform.position;
        Quaternion endQuaternion = saveQuaternion;
        Quaternion startQuaternion = camera.transform.rotation;
        for(float t = 0; t < 1; t += Time.deltaTime*speed){
            camera.transform.position = Vector3.Lerp(startPosition,saveCamPosition,t);
            camera.transform.rotation = Quaternion.Lerp(startQuaternion,saveQuaternion,t);
            yield return null;
        }
        Character.AllowEverything();
    }
    public bool tradingStartedFlag = false;
    // Update is called once per frame
    void Update()
    {
        if (!tradingStartedFlag)
            return;
        if (Input.GetKeyDown(KeyCode.E) || !LightControl.electricityState){
            tradingStartedFlag = false;
            StartCoroutine(outOfTradingSmooth());
            return;
        }
    }
}
