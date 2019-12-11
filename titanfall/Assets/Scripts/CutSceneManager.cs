using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutSceneManager : MonoBehaviour
{
    public Animator titanAnim;
    public Animator playerAnim;
    public Camera fpsCam;
    public CinemachineVirtualCamera cutSceneCam;
    // Start is called before the first frame update
    void Start()
    {
        //fpsCam.enabled = false;
        //cutSceneCam.enabled = true;
        titanAnim = transform.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playVictory()
    {
        playerAnim.SetTrigger("isVictory");
    }
}
