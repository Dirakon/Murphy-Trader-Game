using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUp : MonoBehaviour
{
    public static PopUp singleton;
    public TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake(){
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
