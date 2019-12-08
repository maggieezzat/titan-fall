using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform nozzle;

    public Transform [] patrolPoints;
    private int health = 100;

    PlayerScript playerScript;

    public Vector3 assaultPos = new Vector3(0.1563f, -0.052f, -0.106f);
    public Vector3 assaultRot = new Vector3(-173.68f, 296.309f, 251.662f);

    public Vector3 sniperPos = new Vector3(0.3f, 0.006f, -0.094f);
    public Vector3 sniperRot = new Vector3(0.285f, 306.415f, 299f);

    public Vector3 titanPos = new Vector3(0.005f, 0.324f, 0.045f);
    public Vector3 titanRot = new Vector3(255.477f, 254.701f, -211.86f);

    public GameObject assaultObj;
    public GameObject sniperObj;
    public GameObject titanGun;

    private Animator enemyAnimator;

    private NavMeshAgent agent;
    public ProgressBar healthBar;

    public GameObject player;
    private bool inRange = false;

    bool initPatrolSet = false;
    int currentIndex = 0;
    bool isDead = false;
    public ParticleSystem blood;


    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerScript = PlayerScript.Instance;
    }


    void Update()
    {

        if(health <= 0)
        {
            die();

        }



        if(inRange && !isDead)
        {
            agent.SetDestination(player.transform.position);
            Vector3 dir = Vector3.ProjectOnPlane((player.transform.position - transform.position), Vector3.up);
            transform.rotation = Quaternion.LookRotation(dir);
            if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rifle Run"))
            {
                enemyAnimator.SetBool("isRunning",true);
                enemyAnimator.SetBool("isWalking", false);

            }

            initPatrolSet = false;
                
        }
        else
        {
            enemyAnimator.SetBool("isRunning",false);
            enemyAnimator.SetBool("isWalking", true);

            if(!initPatrolSet){
                agent.destination = patrolPoints[currentIndex].position;
                initPatrolSet = true;

            }
                


            if(agent.remainingDistance <= 1)
            {
                currentIndex = (currentIndex + 1) % 2;
                agent.destination = patrolPoints[currentIndex].position;
            }    
        }

        float rangeX = player.transform.position.x - transform.position.x;
        float rangeZ = player.transform.position.z - transform.position.z;

        //float distance = Mathf.Sqrt(Mathf.Pow(2, rangeX) + Mathf.Pow(2, rangeZ));
        float distance = Vector3.Distance(agent.transform.position, player.transform.position);
        if (distance < 25f)
        {
            inRange = true;
            
            
        }  
        else
        {
            inRange = false;
            //enemyAnimator.SetBool("isForward", false);
        }

        if (gameObject.tag == "EnemyAssault")
        {
            if (enemyAnimator.GetBool("isWalking"))
            {
                assaultObj.gameObject.transform.localPosition = assaultPos;
                assaultObj.gameObject.transform.localEulerAngles = assaultRot;
            }

        }
        else if (gameObject.tag == "EnemySniper")
        {
            if (enemyAnimator.GetBool("isWalking"))
            {
                sniperObj.gameObject.transform.localPosition = sniperPos;
                sniperObj.gameObject.transform.localEulerAngles = sniperRot;
            }
        }

        else if (gameObject.tag == "EnemyTitan")
        {
            if (enemyAnimator.GetBool("isWalking"))
            {
                titanGun.gameObject.transform.localPosition = titanPos;
                titanGun.gameObject.transform.localEulerAngles = titanRot;
            }
        }
        
    }


    public void takeDamage(int damage)
    {
        health -= damage;
        healthBar.BarValue = health;
        enemyAnimator.SetTrigger("isHit");
        blood.Play();
    }

    public void die()
    {
        enemyAnimator.SetTrigger("isDead");
        transform.GetComponent<Collider>().enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;
        isDead = true;

    }
    
    
}
