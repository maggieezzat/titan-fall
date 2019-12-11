using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.EventSystems;

namespace OurNameSpace{

public class CombatLevelManager : MonoBehaviour
{
    public FirstPersonController fps;
    public PlayerScript playerScript;
    public WeaponScript weaponScript;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pilotHUD;
    public GameObject titanHUD;
    public GameObject selectedHUD;

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

    public GameObject defAbilityCoolDown_GO;
    public ProgressBarCircle defAbilityCoolDown;
    public float counter = 15f;
    
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
        selectedHUD = pilotHUD;
        selectedHUD.SetActive(true);

        SetBars();
        SetTexts();
    }

    
    void Update()
    {
        if(defAbilityCoolDown_GO.activeSelf)
        {
            counter -= 1f * Time.deltaTime;
            defAbilityCoolDown.UpdateValue((int)(counter), 15);
        }
        if(counter <=0)
        {
            counter = 15f;
            defAbilityCoolDown_GO.SetActive(false);
        }
        

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
        selectedHUD.SetActive(false);
        audioSource.Stop();
        audioSource.clip = pauseMusic;
        audioSource.Play();
        Time.timeScale = 0;

    }

    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        selectedHUD.SetActive(false);
        audioSource.Stop();
        audioSource.clip = pauseMusic;
        audioSource.Play();
        Time.timeScale = 0;

        fps.gameOver = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        selectedHUD.SetActive(false);
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
        if(selectedHUD == titanHUD)
        {
            titanHealthBar.UpdateValue(playerScript.titanPlayer.health, 400);
            titanDashBar.UpdateValue(fps.dashMeter, 3);
            titanCoreAbilityBar.UpdateValue(playerScript.titanPlayer.coreAbilityMeter, 100);
        }
        else
        {
            pilotHealthBar.UpdateValue(playerScript.pilotPlayer.health, 100);
            pilotTitanFallBar.UpdateValue(playerScript.pilotPlayer.titanFallMeter, 100);
        }
        
    }

    void SetTexts()
    {
        if(selectedHUD == pilotHUD)
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

        }
          
    }

    public void switchToTitanStats()
    {
        titanHUD.SetActive(false);
        pilotHUD.SetActive(false);
        selectedHUD = titanHUD;
        selectedHUD.SetActive(true);
    }

    public void switchToPilotStats()
    {
        titanHUD.SetActive(false);
        pilotHUD.SetActive(false);
        selectedHUD = pilotHUD;
        selectedHUD.SetActive(true);
    }


}
}

