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
    
    public int killedEnemies = 0;

    CutSceneManager cutSceneManager;


    public bool isCoreAbility = false;
    public GameObject coreAbilityShield;
    public bool isDefensiveAbility = false;
    public GameObject defensiveAbilityShield;
    float nextTimeToShield = 0f;

    float regenerationCount = 0f;

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

        cutSceneManager = CutSceneManager.Instance;
    }

#endregion

    void Start()
    {
        fireRate = weaponScript.primaryWeapon.fireRate;
        nextTimeToFire = 0f;
        InvokeRepeating("regenerateHealth", 0f, 1f);
    }

    void regenerateHealth()
    {
        if(regenerationCount >= 3f && currentPlayerType == PlayerType.pilot)
        {
            pilotPlayer.incHealth(5);
        }
    }

    void Update()
    {
        checkForFire();
        checkForReload();
        checkForSwitchWeapon();
        checkForTitanCall();
        checkForEmbarkDisEmbark();
        checkForDefensiveAbility();
        checkForCoreAbility();

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

        regenerationCount += 1f * Time.deltaTime;

    }

    
    void checkForReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && 
        currentPlayerType == PlayerType.pilot && 
        weaponScript.currentWeapon.weaponType == WeaponType.primary)
            ((PrimaryWeapon)weaponScript.currentWeapon).reLoad();
    }


    void checkForFire()
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


    void checkForSwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Z) &&  currentPlayerType==PlayerType.pilot)
        {
            weaponScript.switchWeapon();
        }

    }


    void checkForEmbarkDisEmbark()
    {
        //embark
        if(Input.GetKeyDown(KeyCode.E) && embarkEnabled && currentPlayerType == PlayerType.pilot)
        {
            StartCoroutine(embarkCo());
            calledTitan.SetActive(false);
            embarkEnabled = false;
        }

        //disembark
        if(Input.GetKeyDown(KeyCode.E) && currentPlayerType == PlayerType.titan)
        {
            StartCoroutine(disembarkCo());
        }

    }


    void checkForTitanCall()
    {
        // if(Input.GetKeyDown(KeyCode.Q) && 
        // currentPlayerType == PlayerType.pilot && 
        // ((PilotPlayer)currentPlayer).titanFallMeter >= 100)
        //TODO: return titanfall check
        if(Input.GetKeyDown(KeyCode.Q) && 
        currentPlayerType == PlayerType.pilot )
        {
            cutSceneManager.playCutScene();
        }

    }


    void checkForDefensiveAbility()
    {
        if(Input.GetKeyDown(KeyCode.F) && 
        currentPlayerType == PlayerType.titan && 
        Time.time >= nextTimeToShield && !isDefensiveAbility)
        {
            defensiveAbilityShield.SetActive(true);
            Invoke("stopDefensiveAbility",10f);
            isDefensiveAbility = true;
        }

    }

    void stopDefensiveAbility()
    {
        nextTimeToShield = Time.time + 15f;
        isDefensiveAbility = false;
        CombatLevelManager.Instance.defAbilityCoolDown_GO.SetActive(true);
        defensiveAbilityShield.SetActive(false);
    }


    void checkForCoreAbility()
    {
        if(Input.GetKeyDown(KeyCode.V) && 
        currentPlayerType == PlayerType.titan && 
        ((TitanPlayer)currentPlayer).coreAbilityMeter >= 100)
        {
            isCoreAbility = true;
            titanPlayer.resetCoreAbilityMeter();
            weaponScript.activateCoreAbility();
        }
    }


    public void takeDamage(int damage)
    {
        isDead = currentPlayer.decHealth(damage);
        regenerationCount = 0f;
        StartCoroutine(showHitScreenCo());
    }


    IEnumerator embarkCo()
    {
        blackScreen.SetTrigger("isFadeIn");
        yield return new WaitForSeconds(0.5f);
        pilotPlayer.resetTitanFallMeter();
        currentPlayer = titanPlayer;
        currentPlayerType = PlayerType.titan;
        weaponScript.switchToTitanWeapon();
        fireRate = weaponScript.titanWeapon.fireRate;
        titanScreen.SetActive(true);
        CombatLevelManager.Instance.switchToTitanStats();

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
        CombatLevelManager.Instance.switchToPilotStats();

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

    IEnumerator gameOverCo()
    {
        yield return new WaitForSeconds(0.3f);
        CombatLevelManager.Instance.gameOver();
    }

}

}