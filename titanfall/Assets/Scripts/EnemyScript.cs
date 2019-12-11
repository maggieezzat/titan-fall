using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OurNameSpace{


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
    public Transform[] patrolPoints;
    bool initPatrolSet = false;
    int currentPatrolIndex = 0;
    bool inRange = false;
    private bool isPatroling = true;


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

    public AudioSource audioSource;

    public bool isPlaying = false;
    public float nextTimeToFire = 0f;
    public Weapon enemyWeapon;
    bool continousShooting = false;

    public int killPoints;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerScript = PlayerScript.Instance;

        switch (gameObject.tag)
        {
            case "EnemyAssault":
                {
                    enemyWeapon = new PrimaryWeapon(10, FiringMode.automatic, 10, 35, 65);
                    health = 100;
                }
                break;
            case "EnemySniper":
                {
                    enemyWeapon = new PrimaryWeapon(85, FiringMode.single_shot, 1, 35, 100);
                    health = 100;
                }
                break;
            case "EnemyTitan":
                {
                    health = 400;
                    enemyWeapon = new TitanWeapon(15, FiringMode.automatic, 20);
                }
                break;

        }

        InvokeRepeating("toggleFire", 1f, 3f); //"start" firing every 3 seconds
    }


    void Update()
    {

        if (health <= 0)
        {
            die();
        }

        float distance = Vector3.Distance(agent.transform.position, player.transform.position);

        if (distance <= 25f)
            inRange = true;
        else
            inRange = false;

        if (inRange)
        {
            if (!isDead)
            {
                if (agent.remainingDistance <= 15f)
                    agent.updatePosition = false;
                else
                    agent.updatePosition = true;
                agent.SetDestination(player.transform.position);

                Vector3 dir = Vector3.ProjectOnPlane((player.transform.position - transform.position), Vector3.up);
                transform.rotation = Quaternion.LookRotation(dir);

                if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rifle Run"))
                {
                    enemyAnimator.SetBool("isRunning", true);
                    enemyAnimator.SetBool("isWalking", false);
                    enemyAnimator.SetBool("isIdle", false);
                }
                initPatrolSet = false;
                isPatroling = false;
                //firing is handeled in toggle fire when the player is inRange and !isDead
            }
        }
        else
        {   
            if (!isDead)
            {
                agent.updatePosition = true;

                if (!initPatrolSet)
                {
                    agent.destination = patrolPoints[currentPatrolIndex].position;
                    initPatrolSet = true;
                    isPatroling = true;
                }

                if (isPatroling)
                {
                    enemyAnimator.SetBool("isRunning", false);
                    enemyAnimator.SetBool("isIdle", false);
                    enemyAnimator.SetBool("isWalking", true);
                }

                if (agent.remainingDistance <= 1 && isPatroling)
                {
                    enemyAnimator.SetBool("isWalking", false);
                    enemyAnimator.SetBool("isIdle", true);
                    isPatroling = false;
                    Invoke("switchPatrolPt", 5f);
                }
            }
        }

        switch (gameObject.tag)
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
            case "EnemyTitan":  
                if (enemyAnimator.GetBool("isWalking"))
                {
                    titanGun.gameObject.transform.localPosition = titanPos;
                    titanGun.gameObject.transform.localEulerAngles = titanRot;
                }
                break;

        }

        if (inRange && continousShooting && !isDead)
        {
            float fireRate;
            if (enemyWeapon.weaponType == WeaponType.primary)
                fireRate = ((PrimaryWeapon)enemyWeapon).fireRate;
            else
                fireRate = ((TitanWeapon)enemyWeapon).fireRate;

            if (Time.time >= nextTimeToFire)
            {
                agent.updatePosition = false;
                // enemyAnimator.SetTrigger("isFiring");
                nextTimeToFire = Time.time + 1f / fireRate;
                WeaponScript.Instance.enemyFire(enemyWeapon, nozzle);
                if (!isMuzzlePlaying)
                {
                    muzzleFlash.Play();
                    isMuzzlePlaying = true;
                }
            }
        }

    }

    void switchPatrolPt()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % 2;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        isPatroling = true;
    }

    IEnumerator muzzleFlashStopCo()
    {
        yield return new WaitForSeconds(1f);
        muzzleFlash.Stop();
        isMuzzlePlaying = false;
    }

    void toggleFire() //called every 3 sec
    {
        if (inRange && !isDead)
        {
            float fireRate;
            if (enemyWeapon.weaponType == WeaponType.primary)
                fireRate = ((PrimaryWeapon)enemyWeapon).fireRate;
            else
                fireRate = ((TitanWeapon)enemyWeapon).fireRate;

            if (fireRate == 1f) //shoot once every 3 secs while inRange
            {
                enemyAnimator.SetTrigger("isFiring");
                WeaponScript.Instance.enemyFire(enemyWeapon, nozzle);
                if (!isMuzzlePlaying)
                {
                    muzzleFlash.Play();
                    isMuzzlePlaying = true;
                    StartCoroutine("muzzleFlashStopCo");
                }
            }
            else //continuously shoot for one sec every frame update
            {
                continousShooting = true;
                Invoke("stopContinuousShooting", 1f);
            }
        }
    }

    public void stopContinuousShooting()
    {
        continousShooting = false;
        muzzleFlash.Stop();
        isMuzzlePlaying = false;
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
        if(!isDead)
        {
            enemyAnimator.SetTrigger("isDead");
            transform.GetComponent<Collider>().enabled = false;
            agent.updatePosition = false;
            agent.updateRotation = false;
            isDead = true;
            playerScript.killedEnemies++;
            if(playerScript.currentPlayerType == PlayerType.pilot)
                ((PilotPlayer)playerScript.currentPlayer).incTitanFallMeter(killPoints);
            else
            {
                if( !playerScript.isCoreAbility)
                    ((TitanPlayer)playerScript.currentPlayer).incCoreAbilityMeter(killPoints);
            }
                
        }
        
    }


    void step()
    {
        audioSource.Play();
    }


}


}