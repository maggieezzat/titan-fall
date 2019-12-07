using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public SuperScript superScript;
    PrimaryWeaponName primaryWeaponName;
    HeavyWeaponName heavyWeaponName;
    public PrimaryWeapon primaryWeapon;
    public HeavyWeapon heavyWeapon;
    public TitanWeapon titanWeapon;
    public Weapon currentWeapon;

    public GameObject assaultRifle_GO;
    public GameObject sniperRifle_GO;
    GameObject primaryWeapon_GO;
    public GameObject predatorCanon_GO;
    public GameObject launcher_GO;

    //public List<Pool> pools;
    public Pool [] pools = new Pool [3];
    public Dictionary<string, Queue<GameObject>> poolDict;

    Queue<GameObject> heavyWeaponPool;
    int heavyWeaponIndex = 0;

    // public GameObject missilePrefab;
    // public GameObject grenadePrefab;
    // private GameObject heavyWeaponPrefab;

    ParticleSystem muzzleFlashPrimary;
    ParticleSystem muzzleFlashTitan;
    bool isPlaying = false;

    public Camera fpsCamera;


    public static WeaponScript Instance;

#region Singleton
    
    void Awake(){

        Instance = this;

        //superScript = SuperScript.Instance;
        //primaryWeaponName = superScript.primaryWeaponName;
        //heavyWeaponName = superScript.heavyWeaponName;

        primaryWeaponName = PrimaryWeaponName.assaultRifle;
        heavyWeaponName = HeavyWeaponName.rocketLauncher;

        createPrimaryWeapon(primaryWeaponName);
        createHeavyWeapon(heavyWeaponName);
        createTitanWeapon();

        currentWeapon = primaryWeapon;
        //currentWeapon = heavyWeapon;
        //currentWeapon = titanWeapon;
    }

#endregion

    void Start()
    {

        assaultRifle_GO.SetActive(false);
        sniperRifle_GO.SetActive(false);
        
        if(primaryWeaponName == PrimaryWeaponName.assaultRifle)
        {
            primaryWeapon_GO = assaultRifle_GO;
            primaryWeapon_GO.SetActive(true);
            muzzleFlashPrimary = assaultRifle_GO.transform.GetChild(0).GetComponent<ParticleSystem>();
        }
        else
        {
            primaryWeapon_GO = sniperRifle_GO;
            primaryWeapon_GO.SetActive(true);
            muzzleFlashPrimary = sniperRifle_GO.transform.GetChild(0).GetComponent<ParticleSystem>();
        }

        poolDict = new Dictionary<string, Queue<GameObject>>();
        createPools();
        
        if(heavyWeaponName == HeavyWeaponName.grenadeLauncher)
        {
            heavyWeaponPool = poolDict["GrenadesPool"];
            heavyWeaponIndex = 1;
            //pools[heavyWeaponIndex].pool_GO.SetActive(true);
            // heavyWeaponPrefab = grenadePrefab;
        }
        else
        {
            heavyWeaponPool = poolDict["MissilesPool"];
            heavyWeaponIndex = 0;
            //pools[heavyWeaponIndex].pool_GO.SetActive(true);
            // heavyWeaponPrefab = missilePrefab;
        }

        muzzleFlashTitan = predatorCanon_GO.transform.GetChild(0).GetComponent<ParticleSystem>();
        launcher_GO.SetActive(false);
        predatorCanon_GO.SetActive(false);

        muzzleFlashPrimary.Stop();
        muzzleFlashTitan.Stop();
        
    }

    void Update()
    {
        if( (Input.GetButtonUp("Fire1") && currentWeapon.weaponType == WeaponType.primary && primaryWeapon.firingMode == FiringMode.automatic)
        || (Input.GetButtonUp("Fire1") &&  currentWeapon.weaponType == WeaponType.titan))
        {
            StartCoroutine("muzzleFlashStopCo");
            isPlaying = false;
        }
        
    }

    void createPools()
    {
        foreach( Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            Transform parent = pool.pool_GO.transform;

            for(int i=0; i<parent.childCount; i++)
            {
                GameObject obj = parent.GetChild(i).gameObject;
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objectPool);
            pool.pool_GO.SetActive(false);

        }
        pools[2].pool_GO.SetActive(true);
    }

    public void setInactive(GameObject gameObj, Vector3 position, Quaternion rotation)
    {
        gameObj.SetActive(false);
        gameObj.transform.SetParent(pools[heavyWeaponIndex].pool_GO.transform);
        Vector3 collisionLoc = gameObj.transform.position;
        gameObj.transform.localPosition = position;
        gameObj.transform.localRotation = rotation;
        heavyWeaponPool.Enqueue(gameObj);

        //create explosion
        Queue<GameObject> explosions = poolDict["ExplosionsPool"];
        GameObject exp = explosions.Dequeue();
        exp.transform.position = collisionLoc;
        exp.transform.SetParent(null);
        exp.SetActive(true);
        explosions.Enqueue(exp);
        StartCoroutine(nextExplosionCo(exp));

        explosionDamage(collisionLoc, ((HeavyWeapon)currentWeapon).explosionRadius,((HeavyWeapon)currentWeapon).damageAmount);
        
    }

    void explosionDamage(Vector3 center, float radius, int damageAmount)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].transform.tag.Contains("Enemy"))
            {
                hitColliders[i].transform.GetComponent<EnemyScript>().takeDamage(damageAmount);
            }
            i++;
            
        }
    }

    public void switchWeapon()
    {
        if(currentWeapon.weaponType == WeaponType.primary)
        {
            currentWeapon = heavyWeapon;
            primaryWeapon_GO.SetActive(false);
            pools[heavyWeaponIndex].pool_GO.SetActive(true);
            launcher_GO.SetActive(true);
        }
        else
        {
            currentWeapon = primaryWeapon;
            pools[heavyWeaponIndex].pool_GO.SetActive(false);
            launcher_GO.SetActive(false);
            primaryWeapon_GO.SetActive(true);
        }
    }

    public void switchToTitanWeapon()
    {
        currentWeapon = titanWeapon;
        primaryWeapon_GO.SetActive(false);
        pools[heavyWeaponIndex].pool_GO.SetActive(false);
        launcher_GO.SetActive(false);

        predatorCanon_GO.SetActive(true);

    }

    public void playerFire()
    {
        RaycastHit hit;
        if(currentWeapon.weaponType == WeaponType.primary)
        {
            bool RaycastDown = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, 
                out hit, ((PrimaryWeapon)currentWeapon).range);
            
            if(RaycastDown  && hit.transform.tag.Contains("Enemy"))
            {
               hit.transform.GetComponent<EnemyScript>().takeDamage(((PrimaryWeapon)currentWeapon).damageAmount);
            }
            if(primaryWeapon.firingMode == FiringMode.automatic &&  isPlaying == false)
            {
                muzzleFlashPrimary.Play();
                isPlaying = true;
            }
            else if(primaryWeapon.firingMode == FiringMode.single_shot)
            {
                muzzleFlashPrimary.Play();
            }
            
        }
        
        if(currentWeapon.weaponType == WeaponType.heavy)
        {
            Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);
            bool RaycastDown = Physics.Raycast(ray, out hit);
            
            if(RaycastDown && heavyWeaponPool.Peek().activeSelf)
            {
                GameObject projectile = heavyWeaponPool.Dequeue();
                
                projectile.SetActive(true);
                projectile.transform.parent = null;
                projectile.GetComponent<Projectile>().launch(hit.point);
                Debug.Log(hit.transform.name);
                
                StartCoroutine("nextLaunchCo");

                //TODO: damage to enemies within range
            }
        }

        if(currentWeapon.weaponType == WeaponType.titan)
        {
            bool RaycastDown = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, 
                out hit);

            if(RaycastDown  && hit.transform.tag.Contains("Enemy")){
               hit.transform.GetComponent<EnemyScript>().takeDamage(((TitanWeapon)currentWeapon).damageAmount);
            }
            if(isPlaying == false){
                muzzleFlashTitan.Play();
                isPlaying = true;
            }
            
        }

    }

    public void enemyFire(Transform nozzle)
    {
        
    }

    public IEnumerator muzzleFlashStopCo()
    {
        yield return new WaitForSeconds(0.2f);
        muzzleFlashPrimary.Stop();
        muzzleFlashTitan.Stop();
    }

    public IEnumerator nextLaunchCo()
    {
        yield return new WaitForSeconds(0.5f);
        heavyWeaponPool.Peek().SetActive(true);
    }

    public IEnumerator nextExplosionCo(GameObject exp)
    {
        yield return new WaitForSeconds(0.5f);
        exp.transform.SetParent(pools[2].pool_GO.transform);
        exp.SetActive(false);

    }


    public void createPrimaryWeapon(PrimaryWeaponName primaryWeaponName)
    {
        switch(primaryWeaponName){
            case PrimaryWeaponName.assaultRifle: primaryWeapon 
                = new PrimaryWeapon(10,FiringMode.automatic,10,35,65); break;
            default: primaryWeapon = new PrimaryWeapon(85,FiringMode.single_shot,1,6,100); break; //sniper rifle is default
        }
    }

    public void createHeavyWeapon(HeavyWeaponName heavyWeaponName)
    {
        switch(heavyWeaponName){
            case HeavyWeaponName.grenadeLauncher : heavyWeapon = new HeavyWeapon(125,ProjectileMode.curveDown,4); break;
            default: heavyWeapon = new HeavyWeapon(150,ProjectileMode.straightLine,3); break; //rocket launcher is default
        }
    }

    public void createTitanWeapon()
    {
        titanWeapon = new TitanWeapon(15,FiringMode.automatic, 20);
        
    }
}
