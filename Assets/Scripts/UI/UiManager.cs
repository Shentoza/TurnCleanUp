using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UiManager : MonoBehaviour {


    //dummys
   public bool isPlayer1;
   public int player1AP;
   public int player2AP;

    GameObject player1;
    GameObject player2;

    InventorySystem inventSys;
    ManagerSystem managerSys;
    public int maxAP;

    GUIStyle style;

    public inputSystem input;

    // aktionen enum
    public AttributeComponent activeUnit;
    public List<Enums.Actions> activeUnitSkills;

    public Enums.Actions activeSkill = 0;

    private bool figureSelected = false;

    GameObject selected_Unit;
    
	// Use this for initialization
	void Start () {

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        managerSys = GameObject.Find("Manager").GetComponent<ManagerSystem>();
        inventSys = GameObject.Find("Manager").GetComponent<InventorySystem>();
        player1AP = player1.GetComponent<PlayerComponent>().actionPoints;
        player2AP = player2.GetComponent<PlayerComponent>().actionPoints;

        //test angaben
        isPlayer1 = managerSys.getPlayerTurn();       


        //getActiveUnitSkills
        activeUnit = managerSys.selectedFigurine.GetComponent<AttributeComponent>();
        activeUnitSkills = activeUnit.skills;

        //setStyle
        style = new GUIStyle();

        if (isPlayer1)
            input = player1.GetComponent<inputSystem>();
        else
            input = player2.GetComponent<inputSystem>();

        UnitSelectionEvent.OnUnitSelection += UnitSelection;
    }

    void OnDestroy()
    {
        UnitSelectionEvent.OnUnitSelection -= UnitSelection;
    }
	

    void UnitSelection(GameObject unit)
    {
        selected_Unit = unit;
    }

    // Update is called once per frame
    void Update()
    {
        isPlayer1 = managerSys.getPlayerTurn();
        player1AP = player1.GetComponent<PlayerComponent>().actionPoints;
        player2AP = player2.GetComponent<PlayerComponent>().actionPoints;
        if (isPlayer1)
            input = player1.GetComponent<inputSystem>();
        else
            input = player2.GetComponent<inputSystem>();

        if (managerSys.selectedFigurine != null && figureSelected == false)
        {
            figureSelected = true;
            activeUnit = selected_Unit.GetComponent<AttributeComponent>();
        }
        
        //beschaffe aktive einheit
        if (activeUnit)
        {
            activeUnit = selected_Unit.GetComponent<AttributeComponent>();
            activeUnitSkills = activeUnit.skills;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            actionCancel();
        }

    }


   

    // verhindert das zu viele waffenoptionen angezeigt werden
    public List<Enums.Actions> getActiveUnitSkills()
    {
        List<Enums.Actions> activeSkills = new List<Enums.Actions>();
       
       //kann gehen
        if (activeUnitSkills.Contains(Enums.Actions.Move))
        {
            activeSkills.Add(Enums.Actions.Move);
        }

       //hat Primärwaffe angelegt
        if (activeUnit.items.isPrimary)
        {
            //Schlagwaffe
            if (activeUnitSkills.Contains(Enums.Actions.Hit))
            {
                activeSkills.Add(Enums.Actions.Hit);
            }
            //Schusswaffe
            else
            {
                if (activeUnitSkills.Contains(Enums.Actions.Shoot))
                {
                    activeSkills.Add(Enums.Actions.Shoot);
                    activeSkills.Add(Enums.Actions.Reload);
                }
            }
        }
        //Sekundärwaffe Angelegt
        else
        {
            // schusswaffe
            if (activeUnit.items.secondaryWeaponType != Enums.SecondaryWeapons.None)
            {
                if (activeUnitSkills.Contains(Enums.Actions.Shoot))
                {
                    activeSkills.Add(Enums.Actions.Shoot);
                }
            }
        }

       //können waffen gewechselt werden
        if (activeUnitSkills.Contains(Enums.Actions.ChangeWeapon))
        {
            activeSkills.Add(Enums.Actions.ChangeWeapon);
        }

        //Heal
        if (activeUnitSkills.Contains(Enums.Actions.Heal))
        {
            activeSkills.Add(Enums.Actions.Heal);
        }

        //Molotov
        if (activeUnitSkills.Contains(Enums.Actions.Molotov))
        {
            activeSkills.Add(Enums.Actions.Molotov);
        }

        //Grenade
        if (activeUnitSkills.Contains(Enums.Actions.Grenade))
        {
            activeSkills.Add(Enums.Actions.Grenade);
        }

        //Smoke
        if (activeUnitSkills.Contains(Enums.Actions.Smoke))
        {
            activeSkills.Add(Enums.Actions.Smoke);
        }

        //Teargas
        if (activeUnitSkills.Contains(Enums.Actions.Teargas))
        {
            activeSkills.Add(Enums.Actions.Teargas);
        }


        return activeSkills;
    }

    public GUIStyle getStyle()
    {
        return style;
    }



    public void endTurn()
    {
        managerSys.setPlayerTurn();
        actionCancel();
    }

    public void move() {
        actionCancel();

        activeSkill = Enums.Actions.Move;

        if (isPlayer1)
            player1.GetComponent<PlayerComponent>().useAP();
        else
            player2.GetComponent<PlayerComponent>().useAP();
        Debug.Log("Move Aktion");
        AttributeComponent attr = (AttributeComponent)managerSys.getSelectedFigurine().GetComponent(typeof(AttributeComponent));
        input.cancelActions();
        attr.regenerateMovepoints();
        DijkstraSystem.executeDijsktra(attr.getCurrentCell(), attr.actMovRange, attr.weapon.GetComponent<WeaponComponent>().weaponRange);
    }
    public void hit(){
        actionCancel();
        activeSkill = Enums.Actions.Hit;

        input.angriffAusgewaehlt = true;
    }
    public void shoot()
    {
        actionCancel();
        activeSkill = Enums.Actions.Shoot;
        
        input.angriffAusgewaehlt = true;
    }
    public void reload(){
        actionCancel();
        activeSkill = Enums.Actions.Reload;
        inventSys.reloadAmmo(GameObject.Find("Manager").GetComponent<ManagerSystem>().getSelectedFigurine());
    }
    public void changeWeapon(){
        actionCancel();
        activeSkill = Enums.Actions.ChangeWeapon;
        // Audiofeedback wenn Waffe gewechselt wird
        AudioManager.playMainClick();

        AttributeComponent attr = managerSys.getSelectedFigurine().GetComponent<AttributeComponent>();
        InventoryComponent inv = managerSys.getSelectedFigurine().GetComponent<InventoryComponent>();
        DijkstraSystem.executeDijsktra(attr.getCurrentCell(), attr.actMovRange, attr.weapon.GetComponent<WeaponComponent>().weaponRange);
        inv.isPrimary = !inv.isPrimary;

        attr.model.GetComponent<WeaponHolding>().swapWeapons();
    }
    public void heal() {
        actionCancel();
        activeSkill = Enums.Actions.Heal;
        HealthSystem healthSystem = GameObject.Find("Manager").GetComponent<HealthSystem>();
        if (inventSys.decreaseMedikits(GameObject.Find("Manager").GetComponent<ManagerSystem>().getSelectedFigurine()) > 0)
        {
            // Audiofeedpack wenn heilen klappt
            AudioManager.playMedikit();

            healthSystem.doHeal(null, activeUnit, HealthSystem.MEDIPACK);
        }
    }

    /*
    * Audio nur für Feedback erst einmal hier drin, eigentliche Audio soll bei ausführender Aktion gespielt werden
    */
    public void molotov() {
        actionCancel();
        activeSkill = Enums.Actions.Molotov;
        input.molotovAusgewaehlt = true;     
    }

    public void grenade(){
        actionCancel();
        activeSkill = Enums.Actions.Grenade;
        input.granateAusgewaehlt = true;        
    }

    public void  smoke(){
        actionCancel();
        activeSkill = Enums.Actions.Smoke;
        input.smokeAusgewaehlt = true;
    }
    public void teargas()
    {
        actionCancel();
        activeSkill = Enums.Actions.Teargas;
        input.gasAusgewaehlt = true;
    }


    public void actionCancel()
    {
       // activeSkill = Enums.Actions.Cancel;

        input.cancelActions();
    }
}
