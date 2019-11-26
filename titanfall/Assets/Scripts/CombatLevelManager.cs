using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatLevelManager : MonoBehaviour
{
    public GameObject pausePanel;
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

    string pilotWeaponName = "Rifle";
    int pilotAmmoCount = 10;
    int weaponMaxAmmo = 20;

    int pilotHealth = 20;
    int pilotTitanFall = 80;


    //Titan's HUD
    public ProgressBar titanHealthBar;
    public ProgressBar titanDashBar;
    public ProgressBar titanCoreAbilityBar;

    int titanHealth = 20;
    int titanDash = 50;
    int titanCoreAbility = 70;
    
    void Start()
    {
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
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void SetBars()
    {
        titanHealthBar.BarValue = titanHealth;
        titanDashBar.BarValue = titanDash;
        titanCoreAbilityBar.BarValue = titanCoreAbility;

        pilotHealthBar.BarValue = pilotHealth;
        pilotTitanFallBar.BarValue = pilotTitanFall;
    }

    void SetTexts()
    {
        pilotWeaponNameText.text = pilotWeaponName;
        pilotAmmoCountText.text = "Ammo: " + pilotAmmoCount + "/" + weaponMaxAmmo;

    }


}
