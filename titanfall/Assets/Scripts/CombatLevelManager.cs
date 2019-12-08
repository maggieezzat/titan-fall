using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.EventSystems;

public class CombatLevelManager : MonoBehaviour
{
    public FirstPersonController fps;
    public PlayerScript playerScript;
    public WeaponScript weaponScript;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pilotHUD;
    public GameObject titanHUD;

    public AudioSource audioSource;
    public AudioClip combatLevelMusic;
    public AudioClip pauseMusic;


    //Pilot's HUD
    public ProgressBar pilotHealthBar;
    public ProgressBar pilotTitanFallBar;
    public Text pilotWeaponNameText;
    public Text pilotAmmoCountText;

    //Titan's HUD
    public ProgressBar titanHealthBar;
    public ProgressBar titanDashBar;
    public ProgressBar titanCoreAbilityBar;
    
#region Singleton    
    public static CombatLevelManager Instance;

    void Awake()
    {
        Instance = this;
    }

#endregion


    void Start()
    {
        playerScript = PlayerScript.Instance;
        weaponScript = WeaponScript.Instance;

        titanDashBar.dash = true;

        SetBars();
        SetTexts();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        SetBars();
        SetTexts();

    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        pilotHUD.SetActive(false);
        titanHUD.SetActive(false);
        audioSource.Stop();
        audioSource.clip = pauseMusic;
        audioSource.Play();
        Time.timeScale = 0;

    }

    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        pilotHUD.SetActive(false);
        titanHUD.SetActive(false);
        audioSource.Stop();
        audioSource.clip = pauseMusic;
        audioSource.Play();
        Time.timeScale = 0;

        fps.gameOver = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pilotHUD.SetActive(true);
        titanHUD.SetActive(true);
        audioSource.Stop();
        audioSource.clip = combatLevelMusic;
        audioSource.Play();
        Time.timeScale = 1;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("CombatLevel");
        Time.timeScale = 1;
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void SetBars()
    {
        titanHealthBar.BarValue = playerScript.titanPlayer.health;
        titanDashBar.BarValue = fps.dashMeter;
        titanCoreAbilityBar.BarValue = playerScript.titanPlayer.coreAbilityMeter;

        pilotHealthBar.BarValue = playerScript.pilotPlayer.health;
        pilotTitanFallBar.BarValue = playerScript.pilotPlayer.titanFallMeter;
    }

    void SetTexts()
    {
        if(weaponScript.currentWeapon.weaponType == WeaponType.primary)
        {
            pilotAmmoCountText.text = "Ammo: " + weaponScript.primaryWeapon.ammoCount + "/" + weaponScript.primaryWeapon.maxAmmoCount;
            pilotWeaponNameText.text = weaponScript.primaryWeaponName.ToString().ToUpper().Replace("RIFLE"," RIFLE");
        }
        else if(weaponScript.currentWeapon.weaponType == WeaponType.heavy)
        {
            pilotAmmoCountText.text = "";
            pilotWeaponNameText.text = weaponScript.heavyWeaponName.ToString().ToUpper().Replace("LAUNCHER"," LAUNCHER");
        }
        else
        {
            
        }
    }

    public void switchToTitanStats()
    {
        pilotHUD.SetActive(false);
        titanHUD.SetActive(true);
    }

    public void switchToPilotStats()
    {
        pilotHUD.SetActive(true);
        titanHUD.SetActive(false);
    }


}
