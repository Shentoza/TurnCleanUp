using UnityEngine;
using System.Collections;
using System;

public class MovementSystem : MonoBehaviour {

    AttributeComponent playerAttr;

    private float secondsPerCell;

    ArrayList pfad;
    Cell startingCell;
    Cell targetCell;
    float yHeight;
    bool yHeightSet = false;
    public bool moving;

    float deltaSum;


    //Seconds to turn 360°
    private float turning_secondsNeeded = 1.0f;
    private bool turning_started;
    private float turning_direction;
    
	// Use this for initialization
	void Start () {
        secondsPerCell = 0.5f;
        playerAttr = (AttributeComponent)this.gameObject.GetComponent(typeof(AttributeComponent));
        turning_started = false;
    }

	
	// Update is called once per frame
	void Update () {
        continueMovement();
	}

    /*
    <returns>True, wenn Bewegung begonnen wird. False sonst
    */
    public bool MoveTo(Cell target)
    {
        if (moving)
            return false;

        if (target.dij_GesamtKosten <= playerAttr.actMovRange && target != playerAttr.getCurrentCell())
        {
            if (!yHeightSet)
            {
                yHeight = playerAttr.transform.position.y - playerAttr.getCurrentCell().transform.position.y;
                yHeightSet = true;
            }
            targetCell = target;
            startingCell = playerAttr.getCurrentCell();
            pfad = DijkstraSystem.getPath(playerAttr.getCurrentCell(), target);
            moving = true;
            PlayerAssistanceSystem.resetAllCellColors();
        }
        else
        {
            moving = false;
        }

        return moving;
    }

    void continueMovement()
    {
        if (targetCell == null)
            return;

        if(playerAttr.getCurrentCell() != targetCell)
        {
            Cell currentCell = playerAttr.getCurrentCell();

            Cell nextCell = (Cell)pfad[pfad.Count-1];

            if (!checkRotation(currentCell,nextCell))
                return;

            float progress = Mathf.Clamp01(deltaSum / secondsPerCell);

            float parabelY = 0.0f;
            //Startet?
            if(playerAttr.getCurrentCell() == startingCell)
            {
                //Nächste Zelle das Ziel?
                if(nextCell == targetCell)
                {
                    parabelY = -6.0f * progress * progress + 6.0f * progress +yHeight;
                }
                else
                {
                    parabelY = -4.5f * progress * progress + 5.25f * progress + yHeight;
                }
            }
            else
            {
                //Nächste Zelle das Ziel?
                if(nextCell == targetCell)
                {
                    parabelY = -4.5f * progress * progress + 3.75f * progress + 0.75f + yHeight;
                }
                else
                {
                    parabelY = -3.0f * progress * progress + 3.0f * progress + 0.75f + yHeight;
                }
            }
            Vector3 yVector = new Vector3(0, parabelY, 0);
            Vector3 moveVector = Vector3.Lerp(currentCell.transform.position, nextCell.transform.position, progress);
            playerAttr.transform.position = moveVector + yVector;

            if (progress == 1.0f)
            {
                if (currentCell == targetCell)
                    playerAttr.transform.position = targetCell.transform.position;

                /*
                *   Put audio all over the place, sorry crunchtime
                */
                AudioManager.playRandomWalkingSound();
                
                currentCell.setOccupied(null);
                currentCell = nextCell;

                nextCell.setOccupied(this.gameObject);
                playerAttr.setCurrentCell(nextCell);
                playerAttr.actMovRange--;
                deltaSum = 0.0f;
                pfad.RemoveAt(pfad.Count - 1);

                //Zielerreicht
                if(currentCell == targetCell)
                {
                    moving = false;
                    int moveRange = playerAttr.actMovRange;
                    int attackRange = playerAttr.items.getCurrentWeapon().weaponRange;
                    DijkstraSystem.executeDijsktra(currentCell, moveRange, attackRange);
                    PlayerAssistanceSystem.colorAllCells(moveRange, attackRange);
                }
            }
            deltaSum += Time.deltaTime;
        }
    }

    /*
    Returns: True wenn er in die Richtung des Pfades schaut, false sonst
    Interpoliert zwischen der aktuellen Position, und der RIchtung des Pfades und dreht Playerattr.
    */
    public bool checkRotation(Cell currentCell, Cell targetCell)
    {

        //Richtung, in die der Pfad von currentCell zu  targetCell schaut.
        Vector3 walkingDirection = targetCell.transform.position - currentCell.transform.position;

        //Aktuelle Richtung in die wir schauen
        Vector3 facingDirection = playerAttr.transform.forward;

        float maxRadians = (Time.deltaTime * (float)(Mathf.PI * 2.0)) / turning_secondsNeeded;

        Vector3 result = Vector3.RotateTowards(facingDirection, walkingDirection, maxRadians, 1);
        playerAttr.transform.forward = result;


        float angle = Vector3.Angle(playerAttr.transform.forward.normalized, walkingDirection.normalized);
        return angle == 0.0f;
    }
}
