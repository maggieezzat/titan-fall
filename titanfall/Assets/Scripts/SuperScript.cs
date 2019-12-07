using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum PrimaryWeaponName { sniperRifle , assaultRifle };
public enum HeavyWeaponName { rocketLauncher, grenadeLauncher };
public enum WeaponType { primary, heavy, titan};
public class SuperScript : MonoBehaviour
{

    private static bool created = false;

    public PrimaryWeaponName primaryWeaponName = PrimaryWeaponName.sniperRifle;
    public HeavyWeaponName heavyWeaponName = HeavyWeaponName.rocketLauncher;
    
#region Singleton
    public static SuperScript Instance;
    void Awake()
    {
        Instance = this;
        // Ensure the script is not deleted while loading.
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
#endregion
    void Update()
    {

        
    }

    public void loadScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);

    }

}
public enum ProjectileMode { straightLine, curveDown };
public enum FiringMode { automatic, single_shot };

public class Weapon {
    public int damageAmount;
    public WeaponType weaponType;
    public Weapon(int damageAmount){
        this.damageAmount = damageAmount;
    }
}

public class PrimaryWeapon : Weapon {
    public FiringMode firingMode; 
    public int fireRate;
    public int ammoCount;
    public int range;

    public PrimaryWeapon(int damageAmount, FiringMode firingMode, int fireRate, int ammoCount, int range) : base(damageAmount){
        this.weaponType = WeaponType.primary;
        this.firingMode = firingMode;
        this.fireRate = fireRate;
        this.ammoCount = ammoCount;
        this.range = range;
        this.damageAmount = damageAmount;
    }
}

public class HeavyWeapon : Weapon {
    public ProjectileMode projectileMode; 
    public int explosionRadius;

    public HeavyWeapon(int damageAmount, ProjectileMode projectileMode, int explosionRadius) : base(damageAmount){
        this.weaponType = WeaponType.heavy;
        this.projectileMode = projectileMode;
        this.explosionRadius = explosionRadius;
        this.damageAmount = damageAmount;
    }
}

public class TitanWeapon : Weapon {
    public FiringMode firingMode;
    public int fireRate;

    public TitanWeapon(int damageAmount, FiringMode firingMode, int fireRate) : base(damageAmount){
        this.weaponType = WeaponType.titan;
        this.firingMode = firingMode;
        this.fireRate = fireRate;
        this.damageAmount = damageAmount;
    }
}

[System.Serializable]
public class Pool {

    public string tag;
    public GameObject pool_GO;
    //public GameObject prefab;
    //public int size;

}

