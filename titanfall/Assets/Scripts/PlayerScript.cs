using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public SuperScript superScript;
    public WeaponScript weaponScript;
    // Start is called before the first frame update
    void Start()
    {
        superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForFire();
            
    }

    public void checkForFire()
    {
        if(weaponScript.currentWeapon.weaponType == WeaponType.primary)
        {
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.single_shot)
            {
                if(Input.GetButtonDown("Fire1")){
                    weaponScript.fire();
                }
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic)
            {
                if(Input.GetButton("Fire1")){
                    weaponScript.fire();
                }
            }
        }
    }
}
