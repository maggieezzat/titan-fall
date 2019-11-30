using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public SuperScript superScript;
    public WeaponScript weaponScript;
    
    void Start()
    {
        //superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
    }

    void Update()
    {
        checkForFire();
            
    }

    public void checkForFire()
    {
        if(weaponScript.currentWeapon.weaponType == WeaponType.primary)
        {
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.single_shot && 
            Input.GetButtonDown("Fire1"))
            {
                weaponScript.playerFire();
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic && 
            Input.GetButton("Fire1"))
            {
                weaponScript.playerFire();
            }
            
        }
    }
}
