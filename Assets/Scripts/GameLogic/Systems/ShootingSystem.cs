using UnityEngine;
using System.Collections;

public class ShootingSystem : MonoBehaviour
{
    public static ShootingSystem instance { get; private set; }

    /*
    * Just dummy values
    * TO-DO: Make sure values are chosen so currentShootingAccuracy ends up between [0,1] when hitchance is calculated
    */
    // Layermask is observing Cells (Change in Inspector)
    [SerializeField]
    private LayerMask mask;

    // Defaults
    private const float DEFAULT_ACCURACY = 0.75f;
    private const float NO_BONUS = 0.0f;

    // Bonuses
    private const float SHORT_RANGE_SHOT_BONUS = 0.2f;

    // Maluses
    private const float LONG_RANGE_SHOT_MALUS = -0.15f;
    private const float SMOKE_MALUS = -0.3f;
    private const float LOW_COVER_MALUS = -0.1f;
    private const float HIGH_COVER_MALUS = -0.2f;

    // Range
    private const float MID_RANGE = 11.25f;

    private AttributeComponent currentplayerAttr;
    private WeaponComponent currentPlayerWeapon;
    private PlayerComponent currentPlayerComp;
    private Cell currentPlayerCell;

    private AttributeComponent currentTargetAttr;
    private Cell currentTargetCell;

    private float distanceBetweenPlayers;

    static void Initialize()
    {
        if (instance != null)
            Destroy(instance);
        instance = new ShootingSystem();
    }


   
    public bool shoot(GameObject attacker, GameObject target)
    {
        currentplayerAttr = (AttributeComponent)attacker.GetComponent(typeof(AttributeComponent));
        currentPlayerCell = currentplayerAttr.getCurrentCell();
        currentPlayerWeapon = (WeaponComponent)currentplayerAttr.weapon.GetComponent(typeof(WeaponComponent));
        if(attacker.tag == "FigurSpieler1")
            currentPlayerComp = GameObject.Find("Player1").GetComponent<PlayerComponent>();
        else
            currentPlayerComp = GameObject.Find("Player2").GetComponent<PlayerComponent>();

        currentTargetAttr = (AttributeComponent)target.GetComponent(typeof(AttributeComponent));        
        currentTargetCell = currentTargetAttr.getCurrentCell();
               
        distanceBetweenPlayers = Vector3.Magnitude(currentTargetCell.transform.position - currentPlayerCell.transform.position);
        Debug.Log("Distance between players is " + distanceBetweenPlayers);

        if (playerCanShoot())
        {
            float hitChance = chanceOfHittingTarget();
            Debug.Log("Hitchance: " + hitChance);
            if (hitChance >= Random.value)
            {
                if (currentplayerAttr == null)
                    Debug.Log("currentplayerAttr");
                if (currentPlayerComp == null)
                    Debug.Log("currentPlayerComp");
                if (currentTargetAttr == null)
                    Debug.Log("currentTargetAttr");
                HealthSystem.doDamage(currentplayerAttr, currentPlayerComp, currentTargetAttr, HealthSystem.SHOOT);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    // Can player shoot target
    private bool playerCanShoot()
    {
        if (distanceBetweenPlayers <= currentplayerAttr.attackRange + currentPlayerWeapon.weaponRange            
            && currentplayerAttr.canShoot
            && currentPlayerComp.actionPoints > 0)
        {
            if(currentPlayerWeapon.currentBulletsInMagazine > 0)
            { 
                currentPlayerWeapon.currentBulletsInMagazine--;
                return true;
            }
            else
            {
                Debug.Log("Keine Kugeln im Magazin vorhanden. Bitte nachladen.");
                return false;
            }
        }
        else
        {
            Debug.Log("Schuss nicht möglich");
        }
        
        return false;
    }

    // Calculate chance of hitting enemy, clamped to [0,1]
    private float chanceOfHittingTarget()
    {
        float currentShootingAccuracy = DEFAULT_ACCURACY;

        currentShootingAccuracy += currentPlayerWeapon.weaponAccuracy;

        if(smokeIsObstructingVision())
        {
            currentShootingAccuracy += ShootingSystem.SMOKE_MALUS;
        }

        if(targetIsCovered())
        {
            float coverMalus = generateCoverMalus();
            currentShootingAccuracy += coverMalus;
        }

        float distanceBonusOrMalus = generateDistanceBonusOrMalus();
        currentShootingAccuracy += distanceBonusOrMalus;

        return currentShootingAccuracy;
    }

    /* SMOKE related */
    private bool smokeIsObstructingVision()
    {
        if (currentPlayerCell.smoked)
        {
            return true;
        }
        else
        {
            Ray raycast = new Ray(currentPlayerCell.transform.position - Vector3.up / 8, currentTargetCell.transform.position - currentPlayerCell.transform.position);

            //Mask ist die Maske, die nur Objekte des Layers Cell betrachet
            RaycastHit[] hits = Physics.RaycastAll(raycast, distanceBetweenPlayers, mask);
           
            // Debug
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.GetComponent<Cell>().smoked)
                {
                    Debug.Log(hit.collider.name + " is smoked");
                    return true;
                }
            }
        }

        return false;
    }

    /* COVER related */
    private bool targetIsCovered()
    {
        if (currentPlayerCell.niedrigeDeckung || currentPlayerCell.hoheDeckung)
        {
            return true;
        }
        else
        {
            Ray raycast = new Ray(currentPlayerCell.transform.position - Vector3.up / 8, currentTargetCell.transform.position - currentPlayerCell.transform.position);

            //Mask ist die Maske, die nur Objekte des Layers Cell betrachet
            RaycastHit[] hits = Physics.RaycastAll(raycast, distanceBetweenPlayers, mask);

            // Debug
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<Cell>().niedrigeDeckung || hit.collider.gameObject.GetComponent<Cell>().hoheDeckung)
                {
                    Debug.Log(hit.collider.name + " gives you defense");
                    return true;
                }
            }
            return false;
        }
      }

    private float generateCoverMalus()
    {        
        if (currentTargetAttr.highCover)        
            return ShootingSystem.HIGH_COVER_MALUS;        
        
        return ShootingSystem.LOW_COVER_MALUS;        
    }

    /* DISTANCE related */
    // Short Range [0, 7.5]
    // Mid Range [7.6, 15]
    // Long Range [16, attackRange + weaponRange]
    private float generateDistanceBonusOrMalus()
    {    
        // Short Range Bonus
        if (distanceBetweenPlayers <= ShootingSystem.MID_RANGE - 3.75)
        {
            return ShootingSystem.SHORT_RANGE_SHOT_BONUS;
        }
        // Long Range Malus
        else if (distanceBetweenPlayers > ShootingSystem.MID_RANGE + 3.75)
        {
            return ShootingSystem.LONG_RANGE_SHOT_MALUS;
        }        

        // Default 
        return NO_BONUS;
    }    
}
