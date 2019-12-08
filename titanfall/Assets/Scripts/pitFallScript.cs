using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitFallScript : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnCollisionEnter(Collision other) 
    {
        
        if(other.gameObject.tag.Equals("Player"))
        {
            ParkourLevelManager.Instance.gameOver();
            print("game over");
        }
    }

}
