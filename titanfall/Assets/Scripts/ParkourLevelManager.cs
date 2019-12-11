using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace OurNameSpace{

public class ParkourLevelManager : MonoBehaviour
{

    public FirstPersonController fps;
    public PlayerScript playerScript;
    public WeaponScript weaponScript;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pilotHUD;

    public AudioSource audioSource;
    public AudioClip parkourLevelMusic;
    public AudioClip pauseMusic;


    //Pilot's HUD
    public ProgressBar pilotHealthBar;
    public ProgressBar pilotTitanFallBar;
    public Text pilotWeaponNameText;
    public Text pilotAmmoCountText;


#region Singleton    
    public static ParkourLevelManager Instance;

    void Awake()
    {
        Instance = this;
    }

#endregion

    void Start()
    {
        playerScript = PlayerScript.Instance;
        weaponScript = WeaponScript.Instance;

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
        audioSource.Stop();
        audioSource.clip = pauseMusic;
        audioSource.Play();
        Time.timeScale = 0;

    }

    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        pilotHUD.SetActive(false);
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
        audioSource.Stop();
        audioSource.clip = parkourLevelMusic;
        audioSource.Play();
        Time.timeScale = 1;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("ParkourLevel");
        Time.timeScale = 1;
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void SetBars()
    {
        pilotHealthBar.UpdateValue(playerScript.pilotPlayer.health, 100);
        pilotTitanFallBar.UpdateValue(playerScript.pilotPlayer.titanFallMeter, 100);
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
    }


}
}
