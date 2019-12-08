using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform nozzle;
    private int health = 30;

    PlayerScript playerScript;

    private Vector3 assaultPos = new Vector3(0.1563f, -0.052f, -0.106f);
    private Vector3 assaultRot = new Vector3(-173.68f, 296.309f, 251.662f);

    private Vector3 sniperPos = new Vector3(0.3f, 0.006f, -0.094f);
    private Vector3 sniperRot = new Vector3(0.285f, 306.415f, 299f);

    private Vector3 titanPos = new Vector3(0.005f, 0.324f, 0.045f);
    private Vector3 titanRot = new Vector3(255.477f, 254.701f, -211.86f);

    public GameObject assaultObj;
    public GameObject sniperObj;
    public GameObject titanGun;

    private Animator enemyAnimator;

    private NavMeshAgent agent;


    private GameObject enemy;

    public GameObject player;
    private bool inRange = false;


    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;


    void Start()
    {
        //assaultObj.transform.parent = hand;
        enemyAnimator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;


        playerScript = PlayerScript.Instance;
    }


    void Update()
    {
        if (inRange)
            agent.destination = player.transform.position;

        float rangeX = player.transform.position.x - GetComponent<Transform>().position.x;
        float rangeZ = player.transform.position.z - GetComponent<Transform>().position.z;

        float distance = Mathf.Sqrt(Mathf.Pow(2, rangeX) + Mathf.Pow(2, rangeZ));
        //Debug.Log(distance);
        if (distance < 5)
        {
            inRange = true;
            enemyAnimator.SetBool("isForward", true);
            
        }
            
        else
            inRange = false;

        Vector2 deltaPosition = new Vector2(rangeX, rangeZ);
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        enemyAnimator.SetFloat("velx", velocity.x);
        enemyAnimator.SetFloat("vely", velocity.y);
        
        GetComponent<Transform>().LookAt(player.transform);
        




        if (gameObject.tag == "eassault")
        {
            if (enemyAnimator.GetBool("isForward"))
            {
                assaultObj.gameObject.transform.localPosition = assaultPos;
                assaultObj.gameObject.transform.localEulerAngles = assaultRot;
            }

        }
        
        else if (gameObject.tag == "esniper")
        {
            if (enemyAnimator.GetBool("isForward"))
            {
                sniperObj.gameObject.transform.localPosition = sniperPos;
                sniperObj.gameObject.transform.localEulerAngles = sniperRot;
            }
        }

        else if (gameObject.tag == "etitan")
        {
            if (enemyAnimator.GetBool("isForward"))
            {
                titanGun.gameObject.transform.localPosition = titanPos;
                titanGun.gameObject.transform.localEulerAngles = titanRot;
            }
        }
        
    }


    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void die()
    {

    }
    
    
}
