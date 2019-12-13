using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurNameSpace;

public class ParkourWinPortal : MonoBehaviour
{
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player")){
            PlayerScript.Instance.firstPersonController.setMouseLock(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
    }
}
