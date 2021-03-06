﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{
    public SuperScript superScript;


    public AudioMixer masterMixer;

    public void setMusicLevel(float musicLevel)
    {
        masterMixer.SetFloat("musicVol", musicLevel);
    }

    public void setEffectsLevel(float effectsLevel)
    {
        masterMixer.SetFloat("effectsVol", effectsLevel);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("ChooseLoadouts");

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
