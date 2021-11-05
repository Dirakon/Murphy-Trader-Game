using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InterfaceMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro murhphToDollar, curMurphs, curMoney;
    ScreenButton instantiateButton(GameObject prefab)
    {
        ScreenButton but =  Instantiate(prefab, father.position, father.rotation, father).GetComponent<ScreenButton>();
    
        but.transform.localPosition = new Vector3(Random.Range(minLX, maxLX), lY, Random.Range(minLZ, maxLZ));
        
    
        return but;
    }
    GameObject[] currentButtons;
    public Transform father;
    public float minLZ = -0.73f, maxLZ = 0.73f, minLX = -1.1f, maxLX = 1.1f, lY = 0.83f;
    public GameObject sellPrefab, buyPrefab;
    bool respawnButtonsFlag = false;
    public void BuyClicked()
    {
        respawnButtonsFlag = true;
        if (ActualComputerScreen.currentMoney < ActualComputerScreen.currentBethovenValue){
            // Can't afford to buy???
        }else{

        }
    }
    public void SellClicked()
    {
        respawnButtonsFlag = true;
    }
    public void RespawnButtons()
    {
        if (currentButtons[1] != null)
        {
            Destroy(currentButtons[0]);
        }
        if (currentButtons[0] != null)
        {
            Destroy(currentButtons[1]);
        }
        ScreenButton sell = instantiateButton(sellPrefab), buy = instantiateButton(buyPrefab);
        sell.onClick += SellClicked;
        buy.onClick += BuyClicked;
        currentButtons[0] = sell.gameObject;
        currentButtons[1] = buy.gameObject;

        ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 2] = sell.gameObject;
        ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 1] = buy.gameObject;
    }
    void Start()
    {
        currentButtons = new GameObject[2];
        GameObject[] copy = ActualComputerScreen.singelton.objectsOnScreen;
        ActualComputerScreen.singelton.objectsOnScreen = new GameObject[copy.Length + 2];
        for (int i = 0; i < copy.Length; ++i)
        {
            ActualComputerScreen.singelton.objectsOnScreen[i] = copy[i];
        }
        RespawnButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnButtonsFlag)
        {
            respawnButtonsFlag = false;
            RespawnButtons();
        }
        murhphToDollar.text = "Текущая стоимость: " + ActualComputerScreen.currentBethovenValue.ToString() + "$";

        curMurphs.text = "Куплено Murphies: " + ActualComputerScreen.currentlyBoughtBethovens;
        curMoney.text = "На счету: " + ActualComputerScreen.currentMoney.ToString() + "$";
    }
}
