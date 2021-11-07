using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMer : MonoBehaviour
{
    // Start is called before the first frame update
    public static BGMer singleton = null;
    public bool forceShutDown = false;
    float normalVolume;
    AudioSource audioSource;
    void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            normalVolume = audioSource.volume;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (forceShutDown || !PC.pcState || TradeManager.windowsUpdate)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = normalVolume;
        }
    }
}
