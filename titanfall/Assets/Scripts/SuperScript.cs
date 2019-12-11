using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum PrimaryWeaponName { sniperRifle , assaultRifle };
public enum HeavyWeaponName { rocketLauncher, grenadeLauncher };
public enum WeaponType { primary, heavy, titan};
public enum PlayerType {pilot, titan};
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

}
public enum ProjectileMode { straightLine, curveDown };
public enum FiringMode { automatic, single_shot };

public class Weapon 
{
    public int damageAmount;
    public WeaponType weaponType;
    public Weapon(int damageAmount){
        this.damageAmount = damageAmount;
    }
}

public class PrimaryWeapon : Weapon 
{
    public FiringMode firingMode; 
    public int fireRate;
    public int ammoCount;
    public readonly int maxAmmoCount;
    public int range;

    public PrimaryWeapon(int damageAmount, FiringMode firingMode, int fireRate, int maxAmmoCount, int range) : base(damageAmount){
        this.weaponType = WeaponType.primary;
        this.firingMode = firingMode;
        this.fireRate = fireRate;
        this.maxAmmoCount = maxAmmoCount;
        this.ammoCount = maxAmmoCount;
        this.range = range;
        this.damageAmount = damageAmount;
    }

    public void decAmmo()
    {
        ammoCount-=1;
    }

    public void reLoad()
    {   
        this.ammoCount = this.maxAmmoCount;
    }

}

public class HeavyWeapon : Weapon 
{
    public ProjectileMode projectileMode; 
    public int explosionRadius;

    public HeavyWeapon(int damageAmount, ProjectileMode projectileMode, int explosionRadius) : base(damageAmount){
        this.weaponType = WeaponType.heavy;
        this.projectileMode = projectileMode;
        this.explosionRadius = explosionRadius;
        this.damageAmount = damageAmount;
    }
}

public class TitanWeapon : Weapon 
{
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
public class Pool 
{

    public string tag;
    public GameObject pool_GO;

}


public abstract class Player 
{
    public int health;
    public readonly int maxHealth = 100;
    public Player(int health)
    {
        this.health = health;
    }

    public abstract void incHealth(int value);

    public bool decHealth(int value)
    {
        this.health -= value;

        if(health <= 0)
        {
            health = 0;
            return true;
        }
        return false;
    }

}

public class PilotPlayer: Player
{
    public int titanFallMeter;
    public readonly int titanFallMeterMax = 100;

    public PilotPlayer(): base(100)
    {
        this.titanFallMeter = 0;

    }

    public override void incHealth(int value)
    {
        this.health += value;
        
        if(health>=maxHealth)
            health = maxHealth;
    }

    public void incTitanFallMeter(int value)
    {
        this.titanFallMeter += value;
        if(titanFallMeter>=titanFallMeterMax)
            titanFallMeter = titanFallMeterMax;
    }

    public void resetTitanFallMeter()
    {
        this.titanFallMeter = 0;
    }

}

public class TitanPlayer: Player
{
    public int coreAbilityMeter;
    public readonly int coreAbilityMeterMax = 100;

    public int dashMeter;
    public readonly int dashMeterMax = 3;

    public int defensiveAbilityCoolDown;

    public int defensiveAbilityCoolDownMax = 15; //15 seconds

    public TitanPlayer(): base(400)
    {
        this.health = 400;
        this.coreAbilityMeter = 0;
        this.dashMeter = 3;
        this.defensiveAbilityCoolDown = 0;

    }
    //titan's health does not increase
    public override void incHealth(int value)
    {
        this.health += 0;
    }

    public void incCoreAbilityMeter(int value)
    {
        this.coreAbilityMeter += value;
        if(this.coreAbilityMeter>= this.coreAbilityMeterMax)
            this.coreAbilityMeter = this.coreAbilityMeterMax;

    }

    public void resetCoreAbilityMeter()
    {
        this.coreAbilityMeter = 0;

        if(this.coreAbilityMeter <= 0)
            this.coreAbilityMeter = 0;

    }

    public void incDash()
    {
        this.dashMeter +=1;
        if(this.dashMeter >= this.dashMeterMax)
            this.dashMeter = this.dashMeterMax;
    }

    public void decDash()
    {
        this.dashMeter -=1;
        if(this.dashMeter <= 0)
            this.dashMeter = 0;
    }

}

