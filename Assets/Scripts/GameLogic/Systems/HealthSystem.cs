using UnityEngine;
using System.Collections;
using System;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem instance { get; private set; }

    /* DamageFlags */
    public const int SHOOT = 0;

    /* HealFlags */
    public const int MEDIPACK = 0;

    /* Heals */
    private const int MEDIPACK_HEAL = 25;

    static private int animId_tgetHit;

    public static GameObject Initialize(GameObject toAdd)
    {
        GameObject result;
        if (instance != null)
            Destroy(instance);

        if (toAdd != null)
            result = toAdd;
        else
        {
            result = new GameObject("ShootingSystem");
        }

        instance = result.AddComponent<HealthSystem>();
        animId_tgetHit = Animator.StringToHash("getHit");

        return result;
    }

    /* Generates and inflicts damage if necessary */
    static public void doDamage(AttributeComponent attackingPlayerAttr, PlayerComponent attackingPlayerComp, AttributeComponent damageTakingPlayerAtrr, int damageFlag)
    {
        switch(damageFlag)
        {
            case SHOOT:
                int damage = generateShootDamage(attackingPlayerAttr, damageTakingPlayerAtrr);
                inflictShootDamage(attackingPlayerAttr, attackingPlayerComp, damageTakingPlayerAtrr, damage);                    
                break;

            default:

                break;
        }
    }

    /* Generates and inflicts health if necessary */
    // healingPlayerAttr can be null if not needed
    public static void doHeal(AttributeComponent healingPlayerAttr, AttributeComponent healthTakingPlayerAtrr, int healthFlag)
    {
        switch (healthFlag)
        {
            case MEDIPACK:
                int heal = instance.generateMedipackHeal();
                inflictMedipackHeal(healingPlayerAttr, healthTakingPlayerAtrr, heal);

                break;

            default:

                break;
        }
    }

    /* SHOOT related */
    private static int generateShootDamage(AttributeComponent attackingPlayerAttr, AttributeComponent damageTakingPlayerAtrr)
    {
        WeaponComponent attackingPlayerWeapon = attackingPlayerAttr.items.getCurrentWeapon();
        int damage = attackingPlayerWeapon.damage;

        if (targetHasArmor(damageTakingPlayerAtrr))
        {
            ArmorComponent currentTargetArmor = (ArmorComponent)damageTakingPlayerAtrr.armor.GetComponent(typeof(ArmorComponent));
            damage -= currentTargetArmor.armorValue;
        }

        return damage;
    }

    private static void inflictShootDamage(AttributeComponent attackingPlayerAttr, PlayerComponent attackingPlayerComp, AttributeComponent damageTakingPlayerAttr, int damage)
    {
        Debug.Log("Damage taken : " + damage);
        damageTakingPlayerAttr.hp -= damage;
        attackingPlayerComp.useAP();
        attackingPlayerAttr.canShoot = false;

        //Zeug für Animationen
        damageTakingPlayerAttr.model.GetComponent<Animator>().SetTrigger(animId_tgetHit);

        if (damageTakingPlayerAttr.hp <= 0)
            killFigurine(damageTakingPlayerAttr);
    }

    /* MEDIPACK related */
    private int generateMedipackHeal()
    {
        return MEDIPACK_HEAL;
    }

    private static void inflictMedipackHeal(AttributeComponent healingPlayerAttr, AttributeComponent healthTakingPlayerAtrr, int heal)
    {
        if(healingPlayerAttr != null)
        {
            // TO-DO: Medipack aus Inventar des Heilenden Spielers entfernen
        }
        else
        {
            // TO-DO: Medipack aus Inventar des Heilenden Spielers entfernen
        }

        healthTakingPlayerAtrr.hp += heal;
    }

    
    /* ARMOR related */
    private static bool targetHasArmor(AttributeComponent damageTakingPlayerAtrr)
    {
        if (damageTakingPlayerAtrr.armored)
            return true;

        return false;
    }

    public static void inflictGrenadeDamage(AttributeComponent damageTakingPlayerAttr)
    {
        damageTakingPlayerAttr.hp -= 20;

        //Zeug für Animationen
        damageTakingPlayerAttr.gameObject.GetComponent<Animator>().SetTrigger(animId_tgetHit);
        if (damageTakingPlayerAttr.hp <= 0)
            killFigurine(damageTakingPlayerAttr);
    }

    public static void inflictFireDamage(AttributeComponent damageTakingPlayerAttr)
    {
        damageTakingPlayerAttr.hp -= 10;

        damageTakingPlayerAttr.gameObject.GetComponent<Animator>().SetTrigger(animId_tgetHit);


        if (damageTakingPlayerAttr.hp <= 0)
            killFigurine(damageTakingPlayerAttr);
    }

    public static void inflictGasDamage(AttributeComponent damageTakingPlayerAttr)
    {
        damageTakingPlayerAttr.hp -= 10;

        damageTakingPlayerAttr.gameObject.GetComponent<Animator>().SetTrigger(animId_tgetHit);
        if (damageTakingPlayerAttr.hp <= 0)
            killFigurine(damageTakingPlayerAttr);
    }

    public static void killFigurine(AttributeComponent damageTakingPlayerAttr)
    {
        GameObject figurine = damageTakingPlayerAttr.gameObject;

        FindObjectOfType<ManagerSystem>().removeUnit(figurine, damageTakingPlayerAttr.team);
        Destroy(figurine);
    }
}
