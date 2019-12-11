using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public Animator cutSceneTitanAnim;
    public Animator cutScenePilotAnim;
    public Transform fpsController;
    public Camera fpsCam;
    public Camera cutSceneCam;

    public GameObject calledTitan;
    
    public static CutSceneManager Instance;

#region Singleton
    void Awake()
    {
        Instance = this;
    }

#endregion

    public void playCutScene()
    {
        cutSceneCam.enabled = true;
        fpsCam.enabled = false;
        cutSceneTitanAnim.SetTrigger("isLanding");
    }

    public void stopCutScene()
    {
        fpsCam.enabled = true;
        cutSceneCam.enabled = false;
        calledTitan.SetActive(true);
        calledTitan.transform.position = fpsController.position;
    }

    void playVictory()
    {
        cutScenePilotAnim.SetTrigger("isVictory");
    }
}
