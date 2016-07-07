using UnityEngine;
using System.Collections;

public class inputSystem : MonoBehaviour {

    //Stuff vom Manager
    ManagerSystem manager;
    AbilitySystem abilSys;

    //Stuff von der Aktuell gewählten Figur
	CameraRotationScript rotationScript;

	bool figurGewaehlt;
    bool spielerAmZug;

    //Maske für Raycast
    public LayerMask Cellmask;
    //Aktuelle Zelle über die man hovert
    Cell selectedCell;

    //letzte angewählte Zelle zu der man moven kann
    Cell selectedMovementCell;
    bool changedSelectedCell;
    bool changedSelectedMovementCell;


    //Ausgewaehlte Figur
    GameObject selected_Unit;
    //AttributeComponent der gewaehlten Figur
    AttributeComponent selected_Attributes;
    //Movement der ausgewaehlten Figur
    MovementSystem selected_movement;


    //Bools welche Aktion aktuell ausgewählt is
    public bool movementAusgewaehlt;
	public bool angriffAusgewaehlt;
    public bool smokeAusgewaehlt;
    public bool molotovAusgewaehlt;
    public bool gasAusgewaehlt;
    public bool granateAusgewaehlt;

	// Use this for initialization
	void Start () {
        GameObject managerObj = GameObject.Find("Manager");
        manager = managerObj.GetComponent<ManagerSystem>();
        abilSys = managerObj.GetComponent<AbilitySystem>();


        selectedCell = selectedMovementCell =  null;
        changedSelectedCell = changedSelectedMovementCell = false;

        rotationScript = (CameraRotationScript)FindObjectOfType (typeof(CameraRotationScript));
        UnitSelectionEvent.OnUnitSelection += UnitSelection;
	}

    void OnDestroy()
    {
        UnitSelectionEvent.OnUnitSelection -= UnitSelection;
    }

    void UnitSelection(GameObject unit)
    {
        if (unit == selected_Unit)
            return;

        PlayerAssistanceSystem.ClearThrowPath();
        PlayerAssistanceSystem.ClearWalkPath();
        DijkstraSystem.resetDijkstra();


        selected_Unit = unit;

        if (selected_Unit == null)
        {
            rotationScript.setNewTarget(null);
            figurGewaehlt = false;
        }
        else
        {

            selected_Attributes = selected_Unit.GetComponent<AttributeComponent>();
            selected_movement = selected_Unit.GetComponent<MovementSystem>();
            Cell currentCell = selected_Attributes.getCurrentCell();

            PlayerAssistanceSystem.resetAllCellColors();
            int moveRange = selected_Attributes.actMovRange;
            int attackRange = selected_Attributes.items.getCurrentWeapon().weaponRange;
            DijkstraSystem.executeDijsktra(currentCell, moveRange, attackRange);
            PlayerAssistanceSystem.colorAllCells(moveRange,attackRange);
            rotationScript.setNewTarget(selected_Unit);
            figurGewaehlt = true;
        }
    }
	

	// Update is called once per frame
	void Update () {
        Ray mouseOver = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hover;
        Physics.Raycast(mouseOver, out hover, Mathf.Infinity,Cellmask);
        
        //Cell getroffen?
        if(hover.collider != null)
        {
            Cell tmp = (Cell)hover.collider.gameObject.GetComponent(typeof(Cell));
            changedSelectedCell = selectedCell != tmp;

            //Cellkomponent vorhanden?  und Haben wir die alte Zelle getroffen, oder noch garkeine ausgewählt?
            if(tmp != null && (changedSelectedCell || selectedCell == null))
            {
                if (selectedCell != null)
                {
                    if (figurGewaehlt && !selected_movement.moving)
                        PlayerAssistanceSystem.colorCell(selectedCell, PlayerAssistanceSystem.lastMoveRange, PlayerAssistanceSystem.lastAttackRange);
                    else
                        PlayerAssistanceSystem.resetSingleCell(selectedCell);
                }
                    selectedCell = tmp;
                    PlayerAssistanceSystem.highlightSingleCell(selectedCell);

                if(movementAusgewaehlt)
                {
                    if(selectedCell.dij_GesamtKosten <= selected_Attributes.actMovRange)
                    {
                        changedSelectedMovementCell = selectedMovementCell != selectedCell;
                        selectedMovementCell = selectedCell;
                    }
                }
            }
        }


        if (Input.GetMouseButtonDown (0)) 
		{
            spielerAmZug = manager.getPlayerTurn();  //True = Spieler Eins, False = Spieler zwei
            Ray clicked = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

            Physics.Raycast(clicked, out hit, Mathf.Infinity);
			if(hit.collider != null)
			{                
				if (((hit.collider.gameObject.tag == "FigurSpieler1" && spielerAmZug) 
                    || (hit.collider.gameObject.tag == "FigurSpieler2" && !spielerAmZug)) 
                    && !angriffAusgewaehlt) 
				{
                    //Neuer Spieler angeklickt
					if(selected_Unit != hit.collider.gameObject)
					{
                        UnitSelectionEvent.Send(hit.collider.gameObject);
					}
				}
				if (angriffAusgewaehlt && figurGewaehlt)
				{
					if((hit.collider.gameObject.tag == "FigurSpieler2" && spielerAmZug)
                        || hit.collider.gameObject.tag == "FigurSpieler1" && !spielerAmZug)
					{
                        manager.shoot(selected_Unit, hit.collider.gameObject);
                        angriffAusgewaehlt = false;
					}
                    else
                    {
                        Debug.Log("Kann nicht angegriffen werden");
                    }
                }
				if (smokeAusgewaehlt)
				{
                    if (selectedCell != null && figurGewaehlt)
                    {
                        Debug.Log("Input: Smoke 2");
                        abilSys.throwGrenade(selectedCell, selected_Unit, Enums.Effects.Smoke);
						smokeAusgewaehlt = false;
					}
				}
				if (molotovAusgewaehlt)
				{
                    if(selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, selected_Unit, Enums.Effects.Fire);
                        molotovAusgewaehlt = false;
				}
				}
                if (gasAusgewaehlt)
                {
                    if (selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, selected_Unit, Enums.Effects.Gas);
                        gasAusgewaehlt = false;
                    }

                }
                if (granateAusgewaehlt)
                {
                    if (selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, selected_Unit, Enums.Effects.Explosion);
                        granateAusgewaehlt = false;
                    }

                }
            }
		}

        //Wenn begonnen wird rechts zu klicken
        if(Input.GetMouseButtonDown(1))
        {
            if (figurGewaehlt && selectedCell.dij_GesamtKosten <= selected_Attributes.actMovRange)
            {
                movementAusgewaehlt = true;
                selectedMovementCell = selectedCell;
                ArrayList path = DijkstraSystem.getPath(selected_Attributes.getCurrentCell(), selectedMovementCell);
                PlayerAssistanceSystem.PaintWalkPath(path);
            }
        }

        
        if (Input.GetMouseButton(1))
        {
            if (movementAusgewaehlt && changedSelectedMovementCell)
            { 
                ArrayList path = DijkstraSystem.getPath(selected_Attributes.getCurrentCell(), selectedMovementCell);
                PlayerAssistanceSystem.PaintWalkPath(path);
            }
        }

        if (Input.GetMouseButtonUp (1)) {
            if (movementAusgewaehlt)
            {
                if(selected_movement.MoveTo(selectedMovementCell))
                {
                    movementAusgewaehlt = false;
                    PlayerAssistanceSystem.ClearWalkPath();
                    DijkstraSystem.resetDijkstra();
                }
            }
        }

        if(Input.GetKeyDown("a") && selected_Unit != null)
        {
            angriffAusgewaehlt = !angriffAusgewaehlt;
            if (angriffAusgewaehlt)
                Debug.Log("Angriff ausgewählt");
            else
                Debug.Log("Kein Angriff");
        }
		if (Input.GetKey ("r")) {
			rotationScript.setStartRotation ();
		}
		if (!Input.GetKey ("r")) {
			rotationScript.setStopRotation ();  
		}

		if (Input.GetKeyDown ("s")) {
            selectThrowingGrenade(Enums.Actions.Smoke);
		}
		if (Input.GetKeyDown ("f")) {
            selectThrowingGrenade(Enums.Actions.Molotov);
		}
        if(Input.GetKeyDown("g"))
        {
            selectThrowingGrenade(Enums.Actions.Teargas);
        }
        if (Input.GetKeyDown("d"))
        {
            selectThrowingGrenade(Enums.Actions.Grenade);
        }
        if(Input.GetKeyDown("space"))
        {
			rotationScript.switchCamera();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            cancelActions();
        }
	}

    public void selectThrowingGrenade(Enums.Actions grenadeType)
    {
        cancelActions();
        switch(grenadeType)
        {
            case Enums.Actions.Grenade:
                granateAusgewaehlt = true;
                break;
            case Enums.Actions.Molotov:
                molotovAusgewaehlt = true;
                break;
            case Enums.Actions.Smoke:
                smokeAusgewaehlt = true;
                break;
            case Enums.Actions.Teargas:
                gasAusgewaehlt = true;
                break;
        }
        int attackRange = selected_Attributes.attackRange;
        DijkstraSystem.executeDijsktra(selected_Attributes.getCurrentCell(), 0, attackRange);
        PlayerAssistanceSystem.colorAllCells(0, attackRange);
    }

    public void cancelActions()
    {
        angriffAusgewaehlt = false;
        molotovAusgewaehlt = false;
        smokeAusgewaehlt = false;
        movementAusgewaehlt = false;

        FindObjectOfType<UiManager>().activeSkill = Enums.Actions.Cancel;

        int moveRange = selected_Attributes.actMovRange;
        int attackRange = selected_Attributes.items.getCurrentWeapon().weaponRange;
        DijkstraSystem.executeDijsktra(selected_Attributes.getCurrentCell(), moveRange, attackRange);
        PlayerAssistanceSystem.colorAllCells(moveRange, attackRange);
    }
}

