﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChooseLoadoutsManager : MonoBehaviour
{

    public SuperScript superScript;

    public GameObject titanPanel;
    public GameObject heavyWeaponsPanel;
    public GameObject primaryWeaponsPanel;


    public GameObject selectedPrimaryWeapon;
    public GameObject selectedHeavyWeapon;
    public GameObject selectedTitan;

    public GameObject sniperRifle;
    public GameObject assaultRifle;
    public GameObject primaryWeaponsTextBox;
    public GameObject rocketLauncher;
    public GameObject grenadeLauncher;
    public GameObject heavyWeaponsTextBox;
    public GameObject titan;

    string sniperRifleDesc = "Damage Amount: 85\nFiring Mode: single shot\nFire Rate: 1\nAmmo Count: 6\nRange: 100";
    string assaultRifleDesc = "Damage Amount: 10\nFiring Mode: Automatic\nFire Rate: 10\nAmmo Count: 35\nRange: 65";
    string grenadeLauncherDesc = "Explosion Radius: 4\nExplosion Damage: 125";
    string rocketLauncherDesc = "Explosion Radius: 3\nExplosion Damage: 150";

    TextMeshProUGUI primaryWeaponsText;
    TextMeshProUGUI heavyWeaponsText;



    void Start()
    {
        superScript = GameObject.Find("SuperScript").GetComponent<SuperScript>();
        primaryWeaponsText = primaryWeaponsTextBox.GetComponent<TextMeshProUGUI>();
        heavyWeaponsText = heavyWeaponsTextBox.GetComponent<TextMeshProUGUI>();

        primaryWeaponsPanel.SetActive(true);
        heavyWeaponsPanel.SetActive(false);
        titanPanel.SetActive(false);

        selectedPrimaryWeapon = sniperRifle;
        primaryWeaponsText.text = sniperRifleDesc;
        selectedHeavyWeapon = rocketLauncher;
        heavyWeaponsText.text = rocketLauncherDesc;
        selectedTitan = titan;

        titan.SetActive(false);
        assaultRifle.SetActive(false);
        sniperRifle.SetActive(true);
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(false);

        selectedPrimaryWeapon.SetActive(true);

    }

    
    void Update()
    {
        
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void SelectOnPrimary()
    {
        primaryWeaponsPanel.SetActive(false);
        titanPanel.SetActive(false);
        heavyWeaponsPanel.SetActive(true);

        titan.SetActive(false);
        assaultRifle.SetActive(false);
        sniperRifle.SetActive(false);
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(false);

        selectedPrimaryWeapon.SetActive(false);
        selectedHeavyWeapon.SetActive(true);

    }

    public void BackToPrimary()
    {
        primaryWeaponsPanel.SetActive(true);
        titanPanel.SetActive(false);
        heavyWeaponsPanel.SetActive(false);

        titan.SetActive(false);
        assaultRifle.SetActive(false);
        sniperRifle.SetActive(false);
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(false);

        selectedPrimaryWeapon.SetActive(true);
        selectedHeavyWeapon.SetActive(false);

    }

    public void SelectOnHeavy()
    {
        primaryWeaponsPanel.SetActive(false);
        heavyWeaponsPanel.SetActive(false);
        titanPanel.SetActive(true);

        titan.SetActive(true);
        assaultRifle.SetActive(false);
        sniperRifle.SetActive(false);
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(false);

        selectedPrimaryWeapon.SetActive(false);
        selectedHeavyWeapon.SetActive(false);

    }

    public void BackToHeavy()
    {
        primaryWeaponsPanel.SetActive(false);
        heavyWeaponsPanel.SetActive(true);
        titanPanel.SetActive(false);

        titan.SetActive(false);
        assaultRifle.SetActive(false);
        sniperRifle.SetActive(false);
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(false);

        selectedPrimaryWeapon.SetActive(false);
        selectedHeavyWeapon.SetActive(true);

    }

    public void SelectOnTitan()
    {
        SceneManager.LoadScene("CombatLevel");
    }


    public void selectSniperRifle()
    {
        selectedPrimaryWeapon = sniperRifle;
        sniperRifle.SetActive(true);
        assaultRifle.SetActive(false);
        primaryWeaponsText.text = sniperRifleDesc;
        superScript.primaryWeaponName = PrimaryWeaponName.sniperRifle;

    }

    public void selectAssaultRifle()
    {
        selectedPrimaryWeapon = assaultRifle;
        sniperRifle.SetActive(false);
        assaultRifle.SetActive(true);
        primaryWeaponsText.text = assaultRifleDesc;

        superScript.primaryWeaponName = PrimaryWeaponName.assaultRifle;
        
    }

    public void selectRocketLauncher()
    {
        selectedHeavyWeapon = rocketLauncher;
        grenadeLauncher.SetActive(false);
        rocketLauncher.SetActive(true);
        heavyWeaponsText.text = rocketLauncherDesc;
        superScript.heavyWeaponName = HeavyWeaponName.rocketLauncher;
    }

    public void selectGrenadeLauncher()
    {
        selectedHeavyWeapon = grenadeLauncher;
        grenadeLauncher.SetActive(true);
        rocketLauncher.SetActive(false);
        heavyWeaponsText.text = grenadeLauncherDesc;
        superScript.heavyWeaponName = HeavyWeaponName.grenadeLauncher;
    }


}
