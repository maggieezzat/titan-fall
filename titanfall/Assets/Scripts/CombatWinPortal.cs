﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OurNameSpace{

public class CombatWinPortal : MonoBehaviour
{
    // public PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player") && PlayerScript.Instance.killedEnemies == 15){
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }
}

}