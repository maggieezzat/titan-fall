using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public SuperScript superScript;
    public PrimaryWeapon primaryWeapon;
    public HeavyWeapon heavyWeapon;
    public Weapon currentWeapon;

    public Camera fpsCamera;
    
    void Start()
    {
        //superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
        //createPrimaryWeapon(superScript.primaryWeaponName);
        //createHeavyWeapon(superScript.heavyWeaponName);

        createPrimaryWeapon(PrimaryWeaponName.sniperRifle);
        createHeavyWeapon(HeavyWeaponName.rocketLauncher);
        currentWeapon = primaryWeapon;
    }

    void Update()
    {
    }

    public void switchWeapon()
    {
        if(currentWeapon.weaponType == WeaponType.primary){
            currentWeapon = heavyWeapon;
        }
        else{
            currentWeapon = primaryWeapon;
        }
    }

    public void switchToTitanWeapon()
    {

    }

    public void playerFire()
    {
        RaycastHit hit;
        if(currentWeapon.weaponType == WeaponType.primary){
            Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, 
                out hit, ((PrimaryWeapon)currentWeapon).range);
            if(hit.transform.tag.Contains("Enemy")){
                hit.transform.GetComponent<EnemyScript>().takeDamage(((PrimaryWeapon)currentWeapon).damageAmount);
            }
            
        }
        if(currentWeapon.weaponType == WeaponType.heavy)
        {
            
        }
    }

    public void enemyFire(Transform nozzle)
    {
        
    }


    public void createPrimaryWeapon(PrimaryWeaponName primaryWeaponName)
    {
        switch(primaryWeaponName){
            case PrimaryWeaponName.assaultRifle: primaryWeapon 
                = new PrimaryWeapon(10,FiringMode.automatic,10,35,65); break;
            default: primaryWeapon = new PrimaryWeapon(85,FiringMode.single_shot,1,6,100); break; //sniper rifle is default
        }
    }

    public void createHeavyWeapon(HeavyWeaponName heavyWeaponName)
    {
        switch(heavyWeaponName){
            case HeavyWeaponName.grenadeLauncher : heavyWeapon = new HeavyWeapon(125,ProjectileMode.curveDown,4); break;
            default: heavyWeapon = new HeavyWeapon(150,ProjectileMode.straightLine,3); break; //rocket launcher is default
        }
    }

    public void createTitanWeapon()
    {
        
    }
}
