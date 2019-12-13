using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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

    public CutSceneManager cutSceneManager;


    public bool isCoreAbility = false;
    public GameObject coreAbilityShield;
    public bool isDefensiveAbility = false;
    public GameObject defensiveAbilityShield;
    float nextTimeToShield = 0f;

    float regenerationCount = 0f;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;
    private bool isInvincible;

        public AudioMixerGroup masterAudioMixer;
        public AudioClip[] audioClips;
        public AudioClip[] fpsClips;
        public AudioClip[] titanClips;
        public AudioSource playerAudioSource1;
        public AudioSource playerAudioSource2;

        public AudioSource AddAudio(bool loop, bool playAwake, float maxDistance)
        {
            AudioSource newAudio = gameObject.AddComponent<AudioSource>();
            newAudio.outputAudioMixerGroup = masterAudioMixer;
            newAudio.loop = loop;
            newAudio.playOnAwake = playAwake;
            newAudio.maxDistance = maxDistance;
            return newAudio;
        }
        



        #region Singleton

        public static PlayerScript Instance;
    void Awake()
    {
        Instance = this;

        pilotPlayer = new PilotPlayer();
        titanPlayer = new TitanPlayer();
        currentPlayer = pilotPlayer;
        currentPlayerType = PlayerType.pilot;


        playerAudioSource1 = AddAudio(false, false, 200f);
        playerAudioSource2 = AddAudio(false, false, 200f);
        }

#endregion

    void Start()
        {
        cutSceneManager = CutSceneManager.Instance;
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
        checkForNextLevelCheat();


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
                if (((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.single_shot &&
                Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire
                && ((PrimaryWeapon)weaponScript.currentWeapon).ammoCount > 0)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    weaponScript.playerFire();
                    ((PrimaryWeapon)weaponScript.currentWeapon).decAmmo();
                    if (playerAudioSource2.clip != audioClips[4] || !playerAudioSource2.isPlaying)
                    {
                        playerAudioSource2.clip = audioClips[4];
                        playerAudioSource2.Play();
                    }
            }
            if(((PrimaryWeapon)weaponScript.currentWeapon).firingMode == FiringMode.automatic && 
            Input.GetButton("Fire1") && Time.time >= nextTimeToFire
            && ((PrimaryWeapon)weaponScript.currentWeapon).ammoCount > 0)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                weaponScript.playerFire();
                ((PrimaryWeapon)weaponScript.currentWeapon).decAmmo();
                    if(playerAudioSource2.clip != audioClips[5] || !playerAudioSource2.isPlaying)
                    {
                        playerAudioSource2.clip = audioClips[5];
                        playerAudioSource2.Play();
                    }
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
                    if (playerAudioSource2.clip != audioClips[5] || !playerAudioSource2.isPlaying)
                    {
                        playerAudioSource2.clip = audioClips[5];
                        playerAudioSource2.Play();
                    }
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
        if (
                (Input.GetKeyDown(KeyCode.Q) &&
                currentPlayerType == PlayerType.pilot &&
                ((PilotPlayer)currentPlayer).titanFallMeter >= 100) ||

                (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl) &&      //cheat code for titanfall
                currentPlayerType == PlayerType.pilot && 
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
            )
        {
            playerAudioSource1.clip = audioClips[6];
            playerAudioSource1.Play();
            cutSceneManager.playCutScene();
        }

    }

    void checkForNextLevelCheat()
    {
        if(Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftControl) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
    }


    void checkForDefensiveAbility()
    {
        if(Input.GetKeyDown(KeyCode.F) && 
        currentPlayerType == PlayerType.titan && 
        Time.time >= nextTimeToShield && !isDefensiveAbility && !Input.GetKey(KeyCode.LeftControl))
        {
            defensiveAbilityShield.SetActive(true);
                playerAudioSource1.clip = audioClips[7];
                playerAudioSource1.Play();
            Invoke("stopDefensiveAbility",10f);
            isDefensiveAbility = true;
        }

        if(Input.GetKeyDown(KeyCode.F)
                && Input.GetKey(KeyCode.LeftControl) && currentPlayerType == PlayerType.titan
                && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
        {
            defensiveAbilityShield.SetActive(true);
            playerAudioSource1.clip = audioClips[7];
            playerAudioSource1.Play();
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
        (((TitanPlayer)currentPlayer).coreAbilityMeter >= 100 
        || (Input.GetKey(KeyCode.LeftControl) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)))
        {
            isCoreAbility = true;
            titanPlayer.resetCoreAbilityMeter();
                playerAudioSource1.clip = audioClips[8];
                playerAudioSource1.Play();
            weaponScript.activateCoreAbility();
        }
    }


        public void takeDamage(int damage)
        {
            if (!isDead)
            {
                isDead = currentPlayer.decHealth(damage);
                if (playerAudioSource1.clip != audioClips[2] || !playerAudioSource1.isPlaying)
                {
                    playerAudioSource1.clip = audioClips[2];
                    playerAudioSource1.Play();
                }
                if (playerAudioSource2.clip != audioClips[3] || !playerAudioSource2.isPlaying)
                {
                    playerAudioSource2.clip = audioClips[3];
                    playerAudioSource2.Play();
                }
                regenerationCount = 0f;
                StartCoroutine(showHitScreenCo());
            }
    }


    IEnumerator embarkCo()
    {
        blackScreen.SetTrigger("isFadeIn");
            fpsClips = firstPersonController.m_FootstepSounds;
            firstPersonController.m_FootstepSounds = titanClips;
        yield return new WaitForSeconds(0.5f);
        pilotPlayer.resetTitanFallMeter();
        currentPlayer = titanPlayer;
        currentPlayerType = PlayerType.titan;
        weaponScript.switchToTitanWeapon();
        fireRate = weaponScript.titanWeapon.fireRate;
        titanScreen.SetActive(true);
        CombatLevelManager.Instance.switchToTitanStats();
        // Debug.Log("Should print");
        firstPersonController.embarkTitan();


    }

    IEnumerator disembarkCo()
    {
        blackScreen.SetTrigger("isFadeOut");
            firstPersonController.m_FootstepSounds = fpsClips;
            yield return new WaitForSeconds(1.9f);
        currentPlayer = pilotPlayer;
        currentPlayerType = PlayerType.pilot;
            defensiveAbilityShield.SetActive(false);
            isDefensiveAbility = false;
            weaponScript.switchToPilotWeapon();
        fireRate = weaponScript.primaryWeapon.fireRate;
        titanScreen.SetActive(false);
        CombatLevelManager.Instance.switchToPilotStats();
        firstPersonController.disEmbarkTitan();

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
        if(playerAudioSource1.clip != audioClips[9] || !playerAudioSource1.isPlaying)
        {
            playerAudioSource1.clip = audioClips[9];
            playerAudioSource1.Play();
        }
        yield return new WaitForSeconds(1.5f);
            isDead = false;
        playerAudioSource1.Stop();
        playerAudioSource2.Stop();
        CombatLevelManager.Instance.gameOver();
    }

    public void setIsInvincible(bool value){
        isInvincible = value;
    }

}

}