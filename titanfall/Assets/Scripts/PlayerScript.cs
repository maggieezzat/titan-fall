using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public SuperScript superScript;
    public WeaponScript weaponScript;
    
    private float fireRate;
    private float nextTimeToFire;

    public PlayerType currentPlayerType;
    public Player currentPlayer;
    public PilotPlayer pilotPlayer;
    public TitanPlayer titanPlayer;
    bool isTitan = false;

    public bool isDead = false;
    

#region Singleton

    public static PlayerScript Instance;
    void Awake()
    {
        Instance = this;

        pilotPlayer = new PilotPlayer();
        titanPlayer = new TitanPlayer();
        currentPlayer = pilotPlayer;
        currentPlayerType = PlayerType.pilot;
    }

#endregion

    void Start()
    {
        fireRate = weaponScript.primaryWeapon.fireRate;
        nextTimeToFire = 0f;
    }

    void Update()
    {
        checkForFire();
        
        checkForSwitchWeapon();

        checkForReload();

        //TODO: call titan, embark, disembark
        if(Input.GetKeyDown(KeyCode.T))
        {
            weaponScript.switchToTitanWeapon();
        }

        //TODO: isTitan and isDead => Disembark the titan
        if(isDead && !isTitan)
        {
            //CombatLevelManager.Instance.gameOver();
        }
        if(isDead && isTitan)
        {
            //disembark the titan
            //switch to pilot
            currentPlayer = pilotPlayer;
            isDead = false;
        }

    }

    public void becomeTitan()
    {
        currentPlayer = titanPlayer;
        currentPlayerType = PlayerType.titan;
        weaponScript.switchToTitanWeapon();
        fireRate = weaponScript.titanWeapon.fireRate;

        //TODO: change back again when becoming pilot again
    }
    
    public void checkForReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && weaponScript.currentWeapon.weaponType == WeaponType.primary)
            ((PrimaryWeapon)weaponScript.currentWeapon).reLoad();
    }

    public void checkForFire()
    {
        if(weaponScript.currentWeapon.weaponType == WeaponType.primary && currentPlayerType==PlayerType.pilot)
        {
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.single_shot && 
            Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire 
            && ((PrimaryWeapon)weaponScript.currentWeapon).ammoCount > 0)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
                ((PrimaryWeapon)weaponScript.currentWeapon).decAmmo();
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic && 
            Input.GetButton("Fire1") && Time.time >= nextTimeToFire
            && ((PrimaryWeapon)weaponScript.currentWeapon).ammoCount > 0)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
                ((PrimaryWeapon)weaponScript.currentWeapon).decAmmo();
            }
            
        }
        else if(weaponScript.currentWeapon.weaponType == WeaponType.heavy && currentPlayerType==PlayerType.pilot)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                weaponScript.playerFire();
            }
            
        }
        else if(weaponScript.currentWeapon.weaponType == WeaponType.titan && currentPlayerType==PlayerType.titan)
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
        if(Input.GetKeyDown(KeyCode.Z) &&  currentPlayerType==PlayerType.pilot)
        {
            weaponScript.switchWeapon();
        }

    }

    public void takeDamage(int damage)
    {
        isDead = currentPlayer.decHealth(damage);

    }



}
