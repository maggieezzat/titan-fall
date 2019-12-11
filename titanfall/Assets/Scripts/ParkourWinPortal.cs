using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourWinPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player")){
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
    }
}
