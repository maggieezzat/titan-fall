using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public SuperScript superScript;
    public WeaponScript weaponScript;
    private float fireRate;
    private float nextTimeToFire;
    
    void Start()
    {
        //superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
        fireRate = weaponScript.primaryWeapon.fireRate;
        nextTimeToFire = 0f;
    }

    void Update()
    {
        checkForFire();
        
        checkForSwitchWeapon();

        if(Input.GetKeyDown(KeyCode.T))
        {
            weaponScript.switchToTitanWeapon();
        }
            
    }

    public void becomeTitan()
    {
        fireRate = weaponScript.titanWeapon.fireRate;

        //TODO: change back again when becoming pilot again
    }

    public void checkForFire()
    {
        if(weaponScript.currentWeapon.weaponType == WeaponType.primary)
        {
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.single_shot && 
            Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic && 
            Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
            }
            
        }
        else if(weaponScript.currentWeapon.weaponType == WeaponType.heavy)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                weaponScript.playerFire();
            }
            
        }
        else if(weaponScript.currentWeapon.weaponType == WeaponType.titan)
        {
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
            }

        }
    }


    public void checkForSwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            weaponScript.switchWeapon();
        }

    }



}
