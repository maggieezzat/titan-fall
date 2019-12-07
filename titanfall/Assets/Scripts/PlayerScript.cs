using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static PlayerScript Instance;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        pilotPlayer = new PilotPlayer();
        titanPlayer = new TitanPlayer();
        currentPlayer = pilotPlayer;
        currentPlayerType = PlayerType.pilot;
        
        //superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
        fireRate = weaponScript.primaryWeapon.fireRate;
        nextTimeToFire = 0f;
    }

    void Update()
    {
        checkForFire();
        
        checkForSwitchWeapon();

        checkForReload();

        if(Input.GetKeyDown(KeyCode.T))
        {
            weaponScript.switchToTitanWeapon();
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
                print(((PrimaryWeapon)weaponScript.currentWeapon).ammoCount);
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic && 
            Input.GetButton("Fire1") && Time.time >= nextTimeToFire
            && ((PrimaryWeapon)weaponScript.currentWeapon).ammoCount > 0)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
                ((PrimaryWeapon)weaponScript.currentWeapon).decAmmo();
                print(((PrimaryWeapon)weaponScript.currentWeapon).ammoCount);
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



}
