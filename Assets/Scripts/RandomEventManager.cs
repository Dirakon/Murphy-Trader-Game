using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class RandomEventManager : MonoBehaviour
{
    // Start is called before the first frame update
    RandomEvent[] events = {
        new RandomEvent(LightsGoOff,10),
        new RandomEvent(Police,10),
        new RandomEvent(Friend,10),
        new RandomEvent(NuclearWar,10),
        new RandomEvent(SerialKiller,10),
        new RandomEvent(UpdateGoesOn,10),
        new RandomEvent(Zombie,10),
        new RandomEvent(Vibros,10)
    };
    public float updateChance = 0.1f;
    public static bool nuclearWarHappened = false;
    public static bool eventAllowed = false;
    private static RandomEventManager singleton;
    private static int chanceSum = 0;
    public AudioSource player;
    public AudioClip police, serialKiller,
    friend, intro, lose, win, worldWar,vibrosDeath;
    public AudioClip[] randomActionEndings,zombie,vibros;
    public AudioClip[] randomElectroStart;
    public AudioClip electro2, electro3;
    public AudioClip[] randomUpdateStart;
    public GameObject[] objectsOnFriend;
    public AudioClip update2;
    public AudioClip policeDeath,warDeath,friendDeath,skDeath,zombieDeath;
    private static bool introListenedTo = false;

    void Awake()
    {
        nuclearWarHappened = false;
        eventAllowed = false;
        singleton = this;
        StartCoroutine(Intro());
        chanceSum = 0;
        foreach (var evente in events)
        {
            chanceSum += evente.chance;
        }
    }
    IEnumerator Intro()
    {
        player.clip = intro;
        player.Play();
        if (!introListenedTo)
        {
            while (player.isPlaying)
            {
                yield return null;
            }
        }
        introListenedTo = true;



        eventAllowed = true;
    }
    public static void RollUpdate(){
        if (!eventAllowed)
            return;
        if (UnityEngine.Random.Range(0f,1f) <= singleton.updateChance){
            UpdateGoesOn();
        }
    }
    public void PlayRandomActionEnding(){
        if (UnityEngine.Random.Range(0f,1f)<0.5f){
            player.clip = randomActionEndings[UnityEngine.Random.Range(0,randomActionEndings.Length)];
            player.Play();
        }   
    }
    IEnumerator PoliceIe()
    {
        PopUp.singleton.secondTmPro.text = "Выключи свет и спрячься в шкафу";
        StartCoroutine(pseudoThreadedAudio(police, 2));
        while (LightControl.electricityState)
        {
            if (!isPlaying)
            {
                yield return Died(policeDeath);
            }
            yield return null;
        }
        Shkaf.allowSkhaf = true;

        while (Shkaf.allowSkhaf)
        {
            if (!isPlaying)
            {
                yield return Died(policeDeath);
            }
            yield return null;
        }
        player.Stop();
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool Police()
    {
        if (nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.PoliceIe());


        return true;
    }

    public static void playRandomEvent()
    {
        if (!eventAllowed)
            return;
        int curChance = UnityEngine.Random.Range(0, chanceSum);
        foreach (var evente in singleton.events)
        {
            curChance -= evente.chance;
            if (curChance <= 0)
            {
                if (!evente.runAction())
                {
                    playRandomEvent();
                }
                return;
            }
        }
        Debug.LogError("No event played!");
    }
    void Start()
    {

    }
    public IEnumerator Lost()
    {
        BGMer.singleton.forceShutDown = true;
        PopUp.singleton.secondTmPro.text = "Поражение...";
        eventAllowed = false;
        Character.CancelEverything(false);
        PopUp.singleton.blackScreenOn();
        player.clip = lose;
        player.Play();
        while (player.isPlaying)
        {
            yield return null;
        }
        BGMer.singleton.forceShutDown = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public IEnumerator Won()
    {

        BGMer.singleton.forceShutDown = true;
        PopUp.singleton.secondTmPro.text = "Победа!";
        eventAllowed = false;
        Character.CancelEverything(false);
        PopUp.singleton.blackScreenOn();
        player.clip = win;
        player.Play();
        while (player.isPlaying)
        {
            yield return null;
        }

        BGMer.singleton.forceShutDown = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }
    public IEnumerator Died(AudioClip deathTrack)
    {
        BGMer.singleton.forceShutDown = true;
        PopUp.singleton.secondTmPro.text = "Закон Мёрфи вновь победил...";
        eventAllowed = false;
        Character.CancelEverything(false);
        PopUp.singleton.blackScreenOn();
        if (deathTrack != null){
            player.clip = deathTrack;
            player.Play();
            while (player.isPlaying){
                yield return null;
            }
        }
        BGMer.singleton.forceShutDown = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }
    bool isPlaying = false;
    IEnumerator pseudoThreadedAudio(AudioClip clip, float offsetInSeconds)
    {
        isPlaying = true;
        player.clip = clip;
        player.Play();
        while (player.isPlaying)
        {
            if (eventAllowed)
            {
                isPlaying = false;
                yield break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(offsetInSeconds);
        isPlaying = false;
    }
    IEnumerator FriendIE()
    {
        PopUp.singleton.secondTmPro.text = "Выключи свет и засни на диване";
        StartCoroutine(pseudoThreadedAudio(friend, 2));
        while (LightControl.electricityState)
        {
            if (!isPlaying)
            {
                yield return Died(friendDeath);
            }
            yield return null;
        }
        Coach.allowCoach = true;
        while (Coach.allowCoach)
        {
            if (!isPlaying)
            {
                yield return Died(friendDeath);
            }
            yield return null;
        }
        player.Stop();
        
        eventAllowed = true;
        foreach(var obj in objectsOnFriend){
            obj.SetActive(true);
        }
        PlayRandomActionEnding();
    }
    public static bool Friend()
    {
        if (nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.FriendIE());


        return true;
    }
    IEnumerator SerialKillerIE()
    {
        PopUp.singleton.secondTmPro.text = "Выключи свет и спрячься в шкафу";
        StartCoroutine(pseudoThreadedAudio(serialKiller, 2));
        while (LightControl.electricityState)
        {
            if (!isPlaying)
            {
                yield return Died(skDeath);
            }
            yield return null;
        }
        Shkaf.allowSkhaf = true;
        while (Shkaf.allowSkhaf)
        {
            if (!isPlaying)
            {
                yield return Died(skDeath);
            }
            yield return null;
        }
        player.Stop();
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool SerialKiller()
    {
        if (nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.SerialKillerIE());


        return true;
    }
    IEnumerator NuclearWarIE()
    {
        PopUp.singleton.secondTmPro.text = "Спрячься в шкафу";
        StartCoroutine(pseudoThreadedAudio(worldWar, 0));
        Shkaf.allowSkhaf = true;
        while (Shkaf.allowSkhaf)
        {
            if (!isPlaying)
            {
                Debug.Log("died");
                yield return Died(warDeath);
            }
            yield return null;
        }
        Debug.Log("sruvived");
        player.Stop();
        nuclearWarHappened = true;
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool NuclearWar()
    {
        if (nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.NuclearWarIE());


        return true;
    }
    IEnumerator ZombieIE()
    {
        PopUp.singleton.secondTmPro.text = "Спрячься в шкафу";
        StartCoroutine(pseudoThreadedAudio(zombie[UnityEngine.Random.Range(0,zombie.Length)], 2));
        Shkaf.allowSkhaf = true;
        while (Shkaf.allowSkhaf)
        {
            if (!isPlaying)
            {
                yield return Died(zombieDeath);
            }
            yield return null;
        }
        player.Stop();
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool Zombie()
    {
        if (!nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.ZombieIE());


        return true;
    }
    IEnumerator VibrosIE()
    {
        PopUp.singleton.secondTmPro.text = "Спрячься в шкафу";
        StartCoroutine(pseudoThreadedAudio(vibros[UnityEngine.Random.Range(0,vibros.Length)], 0));
        Shkaf.allowSkhaf = true;
        while (Shkaf.allowSkhaf)
        {
            if (!isPlaying)
            {
                yield return Died(vibrosDeath);
            }
            yield return null;
        }
        player.Stop();
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool Vibros()
    {
        if (!nuclearWarHappened)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.VibrosIE());


        return true;
    }
    IEnumerator LightsGoOffIe()
    {
        PopUp.singleton.secondTmPro.text = "Включи свет";
        player.clip = randomElectroStart[UnityEngine.Random.Range(0, randomElectroStart.Length)];
        player.Play();
        while (player.isPlaying)
        {
            if (LightControl.electricityState)
            {
                break;

            }
            yield return null;
        }
        player.clip = electro2;
        player.Play();
        while (player.isPlaying)
        {
            if (LightControl.electricityState)
            {
                break;

            }
            yield return null;
        }

        while (!LightControl.electricityState) { yield return null; }
        PopUp.singleton.secondTmPro.text = "";
        player.clip = electro3;
        player.Play();
        while (player.isPlaying)
        {
            yield return null;
        }

        eventAllowed = true;
    }
    public static bool LightsGoOff()
    {
        if (!LightControl.electricityState)
            return false;
        eventAllowed = false;
        Rub.ClickOnRub();

        singleton.StartCoroutine(singleton.LightsGoOffIe());


        return true;
    }

    IEnumerator UpdateGoesOnIE()
    {
        PopUp.singleton.secondTmPro.text = "Вздремни на диване";
        player.clip = randomUpdateStart[UnityEngine.Random.Range(0, randomUpdateStart.Length)];
        player.Play();
        Coach.allowCoach = true;
        TradeManager.windowsUpdate = true;
        ActualComputerScreen.singelton.Render();
        while (player.isPlaying)
        {
            if (!Coach.allowCoach)
            {
                break;

            }
            yield return null;
        }
        player.clip = update2;
        player.Play();
        while (player.isPlaying)
        {
            if (!Coach.allowCoach)
            {
                break;

            }
            yield return null;
        }

        while (Coach.allowCoach) { yield return null; }
        TradeManager.windowsUpdate = false;
        ActualComputerScreen.singelton.Render();
        eventAllowed = true;
        PlayRandomActionEnding();
    }
    public static bool UpdateGoesOn()
    {
        if (!LightControl.electricityState || TradeManager.windowsUpdate)
            return false;
        eventAllowed = false;

        singleton.StartCoroutine(singleton.UpdateGoesOnIE());


        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActualComputerScreen.currentMoney < 30f && ActualComputerScreen.currentlyBoughtBethovens == 0 && eventAllowed)
        {
            StartCoroutine(Lost());
        }else if(eventAllowed && ActualComputerScreen.currentMoney >= 10000){
            StartCoroutine(Won());
        }
    }
}


class RandomEvent
{
    public int chance;
    public Func<bool> action;
    public RandomEvent(Func<bool> action, int chance)
    {
        this.chance = chance;
        this.action = action;
    }
    public bool runAction()
    {
        return action.Invoke();
    }
}