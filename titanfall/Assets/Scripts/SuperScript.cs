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
    

    void Awake()
    {
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
    public FiringMode firingMode; // 0 for automatic and 1 for single-shot
    public int fireRate;
    public int ammoCount;
    public int range;

    public PrimaryWeapon(int damageAmount, FiringMode firingMode, int fireRate, int ammoCount, int range) : base(damageAmount){
        this.weaponType = WeaponType.primary;
        this.firingMode = firingMode;
        this.fireRate = fireRate;
        this.ammoCount = ammoCount;
        this.range = range;
    }
}

public class HeavyWeapon : Weapon {
    public ProjectileMode projectileMode; //0 for straight and 1 for curved
    public int explosionRadius;

    public HeavyWeapon(int damageAmount, ProjectileMode projectileMode, int explosionRadius) : base(damageAmount){
        this.weaponType = WeaponType.heavy;
        this.projectileMode = projectileMode;
        this.explosionRadius = explosionRadius;
    }
}

