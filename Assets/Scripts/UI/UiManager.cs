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


    public Enums.Actions activeSkill = 0;



    GameObject selected_Unit;
    AttributeComponent selected_Attributes;
    List<Enums.Actions> selected_Skills;
    public inputSystem selected_input;
    
	// Use this for initialization
	void Start () {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        managerSys = FindObjectOfType<ManagerSystem>();
        inventSys = managerSys.gameObject.GetComponent<InventorySystem>();
        player1AP = player1.GetComponent<PlayerComponent>().actionPoints;
        player2AP = player2.GetComponent<PlayerComponent>().actionPoints;

        //test angaben
        isPlayer1 = managerSys.getPlayerTurn();
        //setStyle
        style = new GUIStyle();

        if (isPlayer1)
            selected_input = player1.GetComponent<inputSystem>();
        else
            selected_input = player2.GetComponent<inputSystem>();

        UnitSelectionEvent.OnUnitSelection += UnitEvent;

        EndturnEvent.OnEndTurn += EndTurn;
        SpendAPEvent.OnAPSpent += SpendAP;
    }

    void OnDestroy()
    {
        UnitSelectionEvent.OnUnitSelection -= UnitEvent;
        EndturnEvent.OnEndTurn -= EndTurn;
        SpendAPEvent.OnAPSpent -= SpendAP;
    }

    void EndTurn(bool PlayerOne)
    {
        actionCancel();
        isPlayer1 = PlayerOne;
        selected_input = isPlayer1 ? player1.GetComponent<inputSystem>() : player2.GetComponent<inputSystem>();
    }
	

    void UnitEvent(GameObject unit)
    {
        selected_Unit = unit;
        if(selected_Unit != null)
        { 
            selected_Attributes = selected_Unit.GetComponent<AttributeComponent>();
            selected_Skills = selected_Attributes.skills;
        }
    }
    
    void SpendAP(int amount, PlayerComponent player)
    {
        if(player.gameObject == player1)
        {
            player1AP -= amount;
        }
        else if(player.gameObject == player2)
        {
            player2AP -= amount;
        }
    }

    // verhindert das zu viele waffenoptionen angezeigt werden
    public List<Enums.Actions> getActiveUnitSkills()
    {
        List<Enums.Actions> activeSkills = new List<Enums.Actions>();
        if (selected_Unit == null)
            return activeSkills;


        //kann gehen
        if (selected_Skills.Contains(Enums.Actions.Move))
        {
            activeSkills.Add(Enums.Actions.Move);
        }

       //hat Primärwaffe angelegt
        if (selected_Attributes.items.isPrimary)
        {
            //Schlagwaffe
            if (selected_Skills.Contains(Enums.Actions.Hit))
            {
                activeSkills.Add(Enums.Actions.Hit);
            }
            //Schusswaffe
            else
            {
                if (selected_Skills.Contains(Enums.Actions.Shoot))
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
            if (selected_Attributes.items.secondaryWeaponType != Enums.SecondaryWeapons.None)
            {
                if (selected_Skills.Contains(Enums.Actions.Shoot))
                {
                    activeSkills.Add(Enums.Actions.Shoot);
                }
            }
        }

       //können waffen gewechselt werden
        if (selected_Skills.Contains(Enums.Actions.ChangeWeapon))
        {
            activeSkills.Add(Enums.Actions.ChangeWeapon);
        }

        //Heal
        if (selected_Skills.Contains(Enums.Actions.Heal))
        {
            activeSkills.Add(Enums.Actions.Heal);
        }

        //Molotov
        if (selected_Skills.Contains(Enums.Actions.Molotov))
        {
            activeSkills.Add(Enums.Actions.Molotov);
        }

        //Grenade
        if (selected_Skills.Contains(Enums.Actions.Grenade))
        {
            activeSkills.Add(Enums.Actions.Grenade);
        }

        //Smoke
        if (selected_Skills.Contains(Enums.Actions.Smoke))
        {
            activeSkills.Add(Enums.Actions.Smoke);
        }

        //Teargas
        if (selected_Skills.Contains(Enums.Actions.Teargas))
        {
            activeSkills.Add(Enums.Actions.Teargas);
        }


        return activeSkills;
    }

    public GUIStyle getStyle()
    {
        return style;
    }

    public void move() {
        actionCancel();

        activeSkill = Enums.Actions.Move;

        if (isPlayer1)
            player1.GetComponent<PlayerComponent>().useAP();
        else
            player2.GetComponent<PlayerComponent>().useAP();
        selected_input.cancelActions();
        selected_Attributes.regenerateMovepoints();

        int moveRange = selected_Attributes.actMovRange;
        int attackRange = selected_Attributes.items.getCurrentWeapon().weaponRange;
        DijkstraSystem.executeDijsktra(selected_Attributes.getCurrentCell(), moveRange, attackRange);
        PlayerAssistanceSystem.colorAllCells(moveRange, attackRange);
    }
    public void hit(){
        actionCancel();
        activeSkill = Enums.Actions.Hit;

        selected_input.angriffAusgewaehlt = true;
    }
    public void shoot()
    {
        actionCancel();
        activeSkill = Enums.Actions.Shoot;
        
        selected_input.angriffAusgewaehlt = true;
    }
    public void reload(){
        actionCancel();
        activeSkill = Enums.Actions.Reload;
        inventSys.reloadAmmo(selected_Unit);
    }
    public void changeWeapon(){
        actionCancel();
        activeSkill = Enums.Actions.ChangeWeapon;
        // Audiofeedback wenn Waffe gewechselt wird
        AudioManager.playMainClick();

        DijkstraSystem.executeDijsktra(selected_Attributes.getCurrentCell(), selected_Attributes.actMovRange, selected_Attributes.items.getCurrentWeapon().weaponRange);
        selected_Attributes.items.isPrimary = !selected_Attributes.items.isPrimary;
        selected_Attributes.model.GetComponent<WeaponHolding>().swapWeapons();
    }
    public void heal() {
        actionCancel();
        activeSkill = Enums.Actions.Heal;
        if (inventSys.decreaseMedikits(selected_Unit) > 0)
        {
            // Audiofeedpack wenn heilen klappt
            AudioManager.playMedikit();

            HealthSystem.doHeal(null, selected_Attributes, HealthSystem.MEDIPACK);
        }
    }

    /*
    * Audio nur für Feedback erst einmal hier drin, eigentliche Audio soll bei ausführender Aktion gespielt werden
    */
    public void molotov() {
        activeSkill = Enums.Actions.Molotov;
        selected_input.selectThrowingGrenade(activeSkill);
    }

    public void grenade(){
        activeSkill = Enums.Actions.Grenade;
        selected_input.selectThrowingGrenade(activeSkill);
    }

    public void  smoke(){
        activeSkill = Enums.Actions.Smoke;
        selected_input.selectThrowingGrenade(activeSkill);
    }
    public void teargas()
    {
        activeSkill = Enums.Actions.Teargas;
        selected_input.selectThrowingGrenade(activeSkill);
    }


    public void actionCancel()
    {
        selected_input.cancelActions();
    }
}
