using UnityEngine;
using System.Collections;
using System;

public class WeaponHolding : MonoBehaviour {

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject leftHandObjectPrimary;
    public GameObject rightHandObjectPrimary;

    private GameObject lhPrimPrefab;
    private GameObject rhPrimPrefab;

    public GameObject leftHandObjectSecondary;
    public GameObject rightHandObjectSecondary;

    private GameObject lhSecondPrefab;
    private GameObject rhSecondPrefab;

    public Enums.Effects selectedGrenade;
    public Enums.Stance primaryStance;
    public Enums.Stance secondaryStance;

    bool primary;

    public GameObject owner { get; set; }

    private AbilitySystem ability;
    private GameObject grenadeInstance;
    private ArmoryComponent armory;
    private Animator anim;

    private int animId_iStance;
    private bool anim_shotIsHit;



	// Use this for initialization
	void Start () {
        ability = FindObjectOfType<AbilitySystem>();
        armory = FindObjectOfType<ArmoryComponent>();
        anim = GetComponent<Animator>();
        animId_iStance = Animator.StringToHash("Stance");
        anim_shotIsHit = false;

        primary = true;
        anim_shotIsHit = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject setLeftHandItem(GameObject item, bool primary)
    {
        GameObject addedItem = null;
        if(primary)
        {
            lhPrimPrefab = item;
            if (leftHandObjectPrimary != null)
                Destroy(leftHandObjectPrimary);
            addedItem = leftHandObjectPrimary = Instantiate(item);
        }
        else
        {
            lhSecondPrefab = item;
            if (leftHandObjectSecondary != null)
                Destroy(leftHandObjectSecondary);
            addedItem = leftHandObjectSecondary = Instantiate(item);
        }
        addedItem.transform.SetParent(leftHand.transform);
        addedItem.transform.localPosition = item.transform.position;
        addedItem.transform.localRotation = item.transform.rotation;
        addedItem.transform.localScale = item.transform.lossyScale;

        return addedItem;
    }

    public GameObject setRightHandItem(GameObject item, bool primary)
    {
        GameObject addedItem = null;
        if(primary)
        {
            rhPrimPrefab = item;
            if (rightHandObjectPrimary != null)
                Destroy(rightHandObjectPrimary);
            addedItem = rightHandObjectPrimary = Instantiate(item);
        }
        else
        {
            rhSecondPrefab = item;
            if (rightHandObjectSecondary != null)
                Destroy(rightHandObjectSecondary);
            addedItem = rightHandObjectSecondary = Instantiate(item);
        }
        addedItem.transform.SetParent(rightHand.transform);
        addedItem.transform.localPosition = item.transform.position;
        addedItem.transform.localRotation = item.transform.rotation;
        addedItem.transform.localScale = item.transform.lossyScale;

        return addedItem;
    }

    public void shoot_isNextShotHit(bool value)
    {
        anim_shotIsHit = value;
    }

    public void shoot_playSound()
    {
        if (anim_shotIsHit)
            AudioManager.playShootingSound(owner.GetComponent<AttributeComponent>().items.getCurrentWeapon());
        else
            AudioManager.playMissed();
    }

    public void initializeEquip(GameObject itemLeftPrim, GameObject itemRightPrim, GameObject itemLeftSec, GameObject itemRightSec, Enums.Stance primStance, Enums.Stance secondStance)
    {
        primary = true;
        if (itemLeftPrim != null)
            setLeftHandItem(itemLeftPrim,true);

        if (itemRightPrim != null)
            setRightHandItem(itemRightPrim,true);

        if (itemLeftSec != null)
            setLeftHandItem(itemLeftSec, false);
        if (itemRightSec != null)
            setRightHandItem(itemRightSec, false);

        primaryStance = primStance;
        secondaryStance = secondStance;
    }

    public void grenade_swapToOther(int starting)
    {
        bool start = starting == 1;
        if (getActiveStance() == Enums.Stance.MeleeRiot)
        {
            getActiveItems()[0].SetActive(!start);
        }

        //Primärwaffe?
        if (primary)
            //Anfang der Animation: Linkes Item in rechte Hand
            if (start)
                leftHandObjectPrimary.transform.SetParent(rightHand.transform);
            //Ende Animation: wieder zurück in Linke
            else
            {
                grenade_resetItem(true);
            }
                

        //Sekundärwaffe
        else
            if (start)
                leftHandObjectSecondary.transform.SetParent(rightHand.transform);
            else
                grenade_resetItem(true);
    }

    public void grenade_resetItem(bool left)
    {
        GameObject resetting;
        GameObject prefab;
        Transform handTransform;
        if (left)
        {
            handTransform = leftHand.transform;
            if(primary)
            {
                resetting = leftHandObjectPrimary;
                prefab = lhPrimPrefab;
            }
            else
            {
                resetting = leftHandObjectSecondary;
                prefab = lhSecondPrefab;
            }
        }
        else
        {
            handTransform = rightHand.transform;
            if (primary)
            {
                resetting = rightHandObjectPrimary;
                prefab = rhPrimPrefab;
            }
            else
            {
                resetting = rightHandObjectSecondary;
                prefab = rhSecondPrefab;
            }
        }
        resetting.transform.SetParent(handTransform);

        resetting.transform.localPosition = prefab.transform.position;
        resetting.transform.localRotation = prefab.transform.rotation;
        resetting.transform.localScale = prefab.transform.lossyScale;
    }


    public void takeIt()
    {

        //Ausm Abilitysystem geworfene Art Granate ziehen
        switch (selectedGrenade)
        {
            case Enums.Effects.Explosion:
                grenadeInstance = (GameObject)Instantiate(armory.grenade);
                break;
            case Enums.Effects.Fire:
                grenadeInstance = (GameObject)Instantiate(armory.molotov);
                break;
            case Enums.Effects.Gas:
                grenadeInstance = (GameObject)Instantiate(armory.smoke);
                break;
            case Enums.Effects.Smoke:
                grenadeInstance = (GameObject)Instantiate(armory.gas);
                break;
        }
        grenadeInstance.transform.position = leftHand.transform.position;
        grenadeInstance.transform.SetParent(leftHand.transform);
    }

    public void throwIt()
    {
        ability.throwIt(grenadeInstance);
        grenadeInstance.transform.SetParent(null);
        if (leftHandObjectPrimary != null)
            leftHandObjectPrimary.SetActive(true);
    }

    public void setNextGrenade(Enums.Effects type)
    {
        selectedGrenade = type;
    }

    public void swapWeapons()
    {
        foreach (GameObject g in getActiveItems())
            if (g != null)
                g.SetActive(false);

        primary = !primary;

        foreach (GameObject g in getActiveItems())
            if (g != null)
                g.SetActive(true);

        anim.SetInteger(animId_iStance, (int) getActiveStance());
    }


    //retrieve active Items
    //[0] -> leftHand
    //[1] -> rightHand
    private GameObject[] getActiveItems()
    {
        GameObject[] value = { null, null };
        if (primary)
        {
            value[0] = leftHandObjectPrimary;
            value[1] = rightHandObjectPrimary;
        }
        else
        {
            value[0] = leftHandObjectSecondary;
            value[1] = rightHandObjectSecondary;
        }

        return value;
    }

    private Enums.Stance getActiveStance()
    {
        Enums.Stance value = Enums.Stance.None;
        if (primary)
            value = primaryStance;
        else
            value = secondaryStance;

        return value;
    }
}


/*Inventory Component besitzt schon viele der Weapon Gameobjects / Types / Primary Secondary etc. pp. */