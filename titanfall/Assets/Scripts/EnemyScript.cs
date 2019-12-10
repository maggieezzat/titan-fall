using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    PlayerScript playerScript;
    public GameObject player;


    public Transform nozzle;
    public ParticleSystem blood;
    public ParticleSystem muzzleFlash;
    bool isMuzzlePlaying = false;


    Animator enemyAnimator;
    NavMeshAgent agent;
    public Transform [] patrolPoints;
    bool initPatrolSet = false;
    int currentPatrolIndex = 0;
    bool inRange = false;
    bool chase = false;

    
    public Vector3 assaultPos = new Vector3(0.1563f, -0.052f, -0.106f);
    public Vector3 assaultRot = new Vector3(-173.68f, 296.309f, 251.662f);

    public Vector3 sniperPos = new Vector3(0.3f, 0.006f, -0.094f);
    public Vector3 sniperRot = new Vector3(0.285f, 306.415f, 299f);

    public Vector3 titanPos = new Vector3(0.005f, 0.324f, 0.045f);
    public Vector3 titanRot = new Vector3(255.477f, 254.701f, -211.86f);


    public GameObject assaultObj;
    public GameObject sniperObj;
    public GameObject titanGun;


    public ProgressBar healthBar;
    int health = 100;
    bool isDead = false;
    
    public AudioClip enemyFootsteps;
    public AudioSource audioSource;
    bool isAudioPlaying = false;

    private bool shoot = false;
    private bool isPatroling = true;
    public bool isPlaying = false;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerScript = PlayerScript.Instance;

        switch(gameObject.tag)
        {
            case "EnemyAssault": health = 100; break;
            case "EnemySniper":health = 100; break;
            case "EnemyTitan": health = 400; break;

        }
        audioSource.clip = enemyFootsteps;
        audioSource.Play();
        isAudioPlaying = true;

        InvokeRepeating("toggleFire", 0, 3);
    }


    void Update()
    {

        if(health <= 0)
        {
            die();
        }

        float rangeX = player.transform.position.x - transform.position.x;
        float rangeZ = player.transform.position.z - transform.position.z;
        float distance = Vector3.Distance(agent.transform.position, player.transform.position);
        
        if (distance < 25f && distance > 15f)
            inRange = true;
        else
            inRange = false;

        if(inRange && !isDead)
        {
            if(!chase)
            {
                agent.SetDestination(player.transform.position);
                chase = true;
            }
            else
            {
                if(agent.remainingDistance >1f)
                    agent.SetDestination(player.transform.position);
            }
            
            //TODO: fire only each 3 seconds
            //TODO: handle CONTINOUS shooting (assault) every 3 seconds??
            WeaponScript.Instance.enemyFire(gameObject.tag, nozzle);
            if(!isMuzzlePlaying){
                muzzleFlash.Play();
                isMuzzlePlaying = true;
                StartCoroutine("muzzleFlashStopCo");
            if (shoot){
                WeaponScript.Instance.enemyFire(gameObject.tag, nozzle);
                if(!isPlaying){
                    muzzleFlash.Play();
                    isPlaying = true;
                    StartCoroutine("muzzleFlashStopCo");
            }

            }
            

            Vector3 dir = Vector3.ProjectOnPlane((player.transform.position - transform.position), Vector3.up);
            transform.rotation = Quaternion.LookRotation(dir);

            if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rifle Run"))
            {
                enemyAnimator.SetBool("isRunning",true);
                enemyAnimator.SetBool("isWalking", false);
                // audioSource.clip = enemyFootsteps;
                // audioSource.Play();
            }
            initPatrolSet = false;   
        }
        else
        {
            chase = false;
            if (isPatroling){
                enemyAnimator.SetBool("isRunning",false);
                enemyAnimator.SetBool("isWalking", true);
            }
            
            // audioSource.clip = enemyFootsteps;
            // audioSource.Play();

            if(!initPatrolSet)
            {
                agent.destination = patrolPoints[currentPatrolIndex].position;
                initPatrolSet = true;
            }
        
            if(agent.remainingDistance <= 1 && isPatroling)
            {
                enemyAnimator.SetBool("isWalking", false);
                enemyAnimator.SetBool("isIdle", true);
                isPatroling = false;
                Invoke("switchPatrolPt", 5f);
            }    
        }

        switch(gameObject.tag)
        {
            case "EnemyAssault": 
                if (enemyAnimator.GetBool("isWalking"))
                {
                    assaultObj.gameObject.transform.localPosition = assaultPos;
                    assaultObj.gameObject.transform.localEulerAngles = assaultRot;
                } 
                break;
            case "EnemySniper":
                if (enemyAnimator.GetBool("isWalking"))
                {
                    sniperObj.gameObject.transform.localPosition = sniperPos;
                    sniperObj.gameObject.transform.localEulerAngles = sniperRot;
                }
                break;
            case "EnemyTitan":  //TODO: Titan Enemy Prefab
                if (enemyAnimator.GetBool("isWalking"))
                {
                    titanGun.gameObject.transform.localPosition = titanPos;
                    titanGun.gameObject.transform.localEulerAngles = titanRot;
                }
                break;

        }
        
    }

    void switchPatrolPt(){
        currentPatrolIndex = (currentPatrolIndex + 1) % 2;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        isPatroling = true;
    }

    IEnumerator muzzleFlashStopCo()
    {
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.Stop();
        isMuzzlePlaying = false;
    }

    void toggleFire(){
        if(inRange && !isDead)
            shoot =!shoot;
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
