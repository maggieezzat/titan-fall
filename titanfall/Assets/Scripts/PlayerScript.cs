using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OurNameSpace{

public class PlayerScript : MonoBehaviour
{
    public GameObject bloodyScreen;
    public GameObject brokenScreen;
    public GameObject titanScreen;
    public Animator blackScreen;
    public SuperScript superScript;
    public WeaponScript weaponScript;
    
    private float fireRate;
    private float nextTimeToFire;

    public PlayerType currentPlayerType;
    public Player currentPlayer;
    public PilotPlayer pilotPlayer;
    public TitanPlayer titanPlayer;

    public bool isDead = false;
    public bool embarkEnabled = false;
    public GameObject calledTitan;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;

    

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

        //embark
        if(Input.GetKeyDown(KeyCode.E) && embarkEnabled && currentPlayerType == PlayerType.pilot)
        {
            StartCoroutine(embarkCo());
            calledTitan.SetActive(false);
            embarkEnabled = false;
            firstPersonController.embarkTitan();
        }

        //disembark
        if(Input.GetKeyDown(KeyCode.E) && currentPlayerType == PlayerType.titan)
        {
            StartCoroutine(disembarkCo());
            firstPersonController.disEmbarkTitan();
        }

        if(isDead && currentPlayerType == PlayerType.pilot)
        {
            bloodyScreen.SetActive(false);
            StartCoroutine(gameOverCo());
        }
        if(isDead && currentPlayerType == PlayerType.titan)
        {
            brokenScreen.SetActive(false);
            isDead = false;
            StartCoroutine(disembarkCo());
        }

    }

    IEnumerator gameOverCo()
    {
        yield return new WaitForSeconds(0.3f);
        CombatLevelManager.Instance.gameOver();
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
        else if(weaponScript.currentWeapon.weaponType == WeaponType.titan )
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
        StartCoroutine(showHitScreenCo());
    }

    public void callTitan()
    {

    }

    IEnumerator embarkCo()
    {
        blackScreen.SetTrigger("isFadeIn");
        yield return new WaitForSeconds(0.5f);
        currentPlayer = titanPlayer;
        currentPlayerType = PlayerType.titan;
        weaponScript.switchToTitanWeapon();
        fireRate = weaponScript.titanWeapon.fireRate;
        titanScreen.SetActive(true);

    }

    IEnumerator disembarkCo()
    {
        blackScreen.SetTrigger("isFadeOut");
        yield return new WaitForSeconds(1.9f);
        currentPlayer = pilotPlayer;
        currentPlayerType = PlayerType.pilot;
        weaponScript.switchToPilotWeapon();
        fireRate = weaponScript.primaryWeapon.fireRate;
        titanScreen.SetActive(false);

    }

    IEnumerator showHitScreenCo()
    {
        if(currentPlayerType == PlayerType.pilot)
            bloodyScreen.SetActive(true);
        else
            brokenScreen.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        bloodyScreen.SetActive(false);
        brokenScreen.SetActive(false);
    }


}

}