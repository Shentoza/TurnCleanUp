using UnityEngine;
using System.Collections;

public class inputSystem : MonoBehaviour {

    //Stuff vom Manager
    ManagerSystem manager;
    AbilitySystem abilSys;

    //Ausgewählte Figur
    GameObject player;
    //Stuff von der Aktuell gewählten Figur
    AttributeComponent attr;
    MovementSystem movement;
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

    GameObject selected_Unit;
    AttributeComponent selected_Attributes;
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
            DijkstraSystem.executeDijsktra(currentCell, attr.actMovRange, attr.items.getCurrentWeapon().weaponRange);
            PlayerAssistanceSystem.colorAllCells();
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
                    if (figurGewaehlt && !movement.moving)
                        PlayerAssistanceSystem.colorCell(selectedCell);
                    else
                        PlayerAssistanceSystem.resetSingleCell(selectedCell);
                }
                    selectedCell = tmp;
                    PlayerAssistanceSystem.highlightSingleCell(selectedCell);

                if(movementAusgewaehlt)
                {
                    if(selectedCell.dij_GesamtKosten <= attr.actMovRange)
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
					if(player != hit.collider.gameObject)
					{
                        UnitSelectionEvent.Send(hit.collider.gameObject);
					}
				}
				if (angriffAusgewaehlt && figurGewaehlt)
				{
					if((hit.collider.gameObject.tag == "FigurSpieler2" && spielerAmZug)
                        || hit.collider.gameObject.tag == "FigurSpieler1" && !spielerAmZug)
					{
                        manager.shoot(player, hit.collider.gameObject);
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
                        abilSys.throwGrenade(selectedCell, player, Enums.Effects.Smoke);
						smokeAusgewaehlt = false;
					}
				}
				if (molotovAusgewaehlt)
				{
                    if(selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, player, Enums.Effects.Fire);
                        molotovAusgewaehlt = false;
				}
				}
                if (gasAusgewaehlt)
                {
                    if (selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, player, Enums.Effects.Gas);
                        gasAusgewaehlt = false;
                    }

                }
                if (granateAusgewaehlt)
                {
                    if (selectedCell != null && figurGewaehlt)
                    {
                        abilSys.throwGrenade(selectedCell, player, Enums.Effects.Explosion);
                        granateAusgewaehlt = false;
                    }

                }
            }
		}

        //Wenn begonnen wird rechts zu klicken
        if(Input.GetMouseButtonDown(1))
        {
            if (figurGewaehlt && selectedCell.dij_GesamtKosten <= attr.actMovRange)
            {
                movementAusgewaehlt = true;
                selectedMovementCell = selectedCell;
                ArrayList path = DijkstraSystem.getPath(attr.getCurrentCell(), selectedMovementCell);
                PlayerAssistanceSystem.PaintWalkPath(path);
            }
        }

        
        if (Input.GetMouseButton(1))
        {
            if (movementAusgewaehlt && changedSelectedMovementCell)
            { 
                ArrayList path = DijkstraSystem.getPath(attr.getCurrentCell(), selectedMovementCell);
                PlayerAssistanceSystem.PaintWalkPath(path);
            }
        }

        if (Input.GetMouseButtonUp (1)) {
            if (movementAusgewaehlt)
            {
                if(movement.MoveTo(selectedMovementCell))
                {
                    movementAusgewaehlt = false;
                    PlayerAssistanceSystem.ClearWalkPath();
                    DijkstraSystem.resetDijkstra();
                }
            }
        }

        if(Input.GetKeyDown("a") && player != null)
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
			smokeAusgewaehlt = true;
		}
		if (Input.GetKeyDown ("f")) {
			molotovAusgewaehlt = true;
		}
        if(Input.GetKeyDown("g"))
        {
            gasAusgewaehlt = true;
        }
        if (Input.GetKeyDown("d"))
        {
            granateAusgewaehlt = true;
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

    public void cancelActions()
    {
        angriffAusgewaehlt = false;
        molotovAusgewaehlt = false;
        smokeAusgewaehlt = false;
        movementAusgewaehlt = false;
        GameObject.Find("UiManager(Clone)").GetComponent<UiManager>().activeSkill = Enums.Actions.Cancel;
    }
}

