using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public Animator titanAnim;
    public Animator playerAnim;
    public Camera fpsCam;
    public Camera cutSceneCam;


    void Start()
    {
        
        titanAnim = transform.GetComponent<Animator>();

    }

    void Update()
    {
        
        
    }

    public void playCutScene()
    {
        cutSceneCam.enabled = true;
        fpsCam.enabled = false;
        titanAnim.SetTrigger("isLanding");
    }

    void playVictory()
    {
        playerAnim.SetTrigger("isVictory");
    }
}
