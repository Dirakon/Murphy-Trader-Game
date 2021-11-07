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
        ScreenButton but = Instantiate(prefab, father.position, father.rotation, father).GetComponent<ScreenButton>();

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
        if (ActualComputerScreen.currentMoney < ActualComputerScreen.currentBethovenValue)
        {
            // Can't afford to buy???
        }
        else
        {
            Character.CameraShake(5f);
            ActualComputerScreen.currentMoney -= ActualComputerScreen.currentBethovenValue;
            ActualComputerScreen.currentlyBoughtBethovens += 1;
            ActualComputerScreen.boughtRecently++;
            StartCoroutine(destroyInTime(Instantiate(clickedEffectPrefab,currentButtons[1].transform.position,Quaternion.identity)));
        }
        respawnButtonsFlag = true;
    }
    public void SellClicked()
    {
        if (ActualComputerScreen.currentlyBoughtBethovens != 0)
        {
            
            Character.CameraShake(5f);
            ActualComputerScreen.currentMoney += ActualComputerScreen.currentBethovenValue;
            ActualComputerScreen.currentlyBoughtBethovens -= 1;
            ActualComputerScreen.soldRecently++;
            StartCoroutine(destroyInTime(Instantiate(clickedEffectPrefab,currentButtons[0].transform.position,Quaternion.identity)));
        }
        respawnButtonsFlag = true;
    }
    public GameObject clickedEffectPrefab;
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
        sell.isSell = true;
        buy.isSell = false;
        ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 2] = sell.gameObject;
        ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 1] = buy.gameObject;
    }
    IEnumerator destroyInTime(GameObject gameObject){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    void Start()
    {
        currentButtons = new GameObject[2];
        
        ScreenButton sell = ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 2].GetComponent<ScreenButton>(), 
        buy = ActualComputerScreen.singelton.objectsOnScreen[ActualComputerScreen.singelton.objectsOnScreen.Length - 1].GetComponent<ScreenButton>();
        currentButtons[0] = sell.gameObject;
        currentButtons[1] = buy.gameObject;
        sell.onClick += SellClicked;
        buy.onClick += BuyClicked;
        sell.isSell = true;
        buy.isSell = false;
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
