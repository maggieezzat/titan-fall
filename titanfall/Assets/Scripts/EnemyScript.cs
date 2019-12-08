using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform nozzle;
    private int health = 30;

    PlayerScript playerScript;

    void Start()
    {
        playerScript = PlayerScript.Instance;
    }

    
    void Update()
    {
        
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void die()
    {

    }
    
    
}
