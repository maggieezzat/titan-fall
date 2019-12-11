using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerScript : MonoBehaviour
{
    PlayerScript playerScript;
    public GameObject message;

    void Start()
    {
        playerScript = PlayerScript.Instance;

    }
    
    void OnTriggerEnter()
    {
        playerScript.embarkEnabled = true;
        message.SetActive(true);

    }

    void OnTriggerExit()
    {
        playerScript.embarkEnabled = false;
        message.SetActive(false);
    }
}
