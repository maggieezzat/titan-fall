using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OurNameSpace{

public class CombatWinPortal : MonoBehaviour
{

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