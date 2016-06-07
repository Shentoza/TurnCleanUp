﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerSystem : MonoBehaviour {
    public int p1UnitCap = 6;
    public int p2UnitCap = 5;

    public List<GameObject> unitListP1;
    public List<GameObject> unitListP2;

    public GameObject activeUnitMarker;

    CameraRotationScript cam;
    public int rounds;             //Spiegelt Rundenzahl wieder
    private bool isPlayer1;         //Spieler1 an der Reihe


    GameObject player1;
    GameObject player2;
    public GameObject selected_Figurine;    //Aktuell ausgewählte Spielfigur
    int roundHalf;  //1 wenn Spieler1 seinen Turn beendet, 2 wenn Spieler2 seinen Turn beendet;


    public GameObject uiManager;
    public GameObject plane;


    public bool uiManagerSet;


    public GameObject unitPrefab;
    public GameObject policePrefab;
    public GameObject rebelPrefab;


   	// Use this for initialization
    void Start () {

        rounds = 0;
        isPlayer1 = true;
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player2.GetComponent<inputSystem>().enabled = false;
        cam = GameObject.Find("Main Camera").GetComponent<CameraRotationScript>();
        plane = GameObject.Find("Plane");
        UnitSelectionEvent.OnUnitSelection += SelectUnit;
    }

    void OnDestroy()
    {
        UnitSelectionEvent.OnUnitSelection -= SelectUnit;
    }
    void SelectUnit(GameObject unit)
    {
        selected_Figurine = unit;
        activeUnitMark();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void startGame()
    {
        Instantiate(uiManager);
        UnitSelectionEvent.Send(unitListP1[0]);
        isPlayer1 = true;
        Camera.main.GetComponent<CameraRotationScript>().enabled = true;

        uiManagerSet = true;
    }


    public void shoot(GameObject attacker, GameObject target)
    {
        AttributeComponent attackAttr = (AttributeComponent) attacker.GetComponent(typeof(AttributeComponent));
        AttributeComponent targetAttr = (AttributeComponent)attacker.GetComponent(typeof(AttributeComponent));

        WeaponHolding weaponHolding = attackAttr.model.GetComponent<WeaponHolding>();
        attackAttr.model_animator.SetTrigger("Shoot");
        if (ShootingSystem.instance.shoot(attacker, target))
        {
            Debug.Log("HIIIIT!!!");
            targetAttr.model_animator.SetTrigger("getHit");
            weaponHolding.shoot_isNextShotHit(true);
            Debug.Log("Getroffen");
        }
        else
        {
            weaponHolding.shoot_isNextShotHit(false);
            Debug.Log("Nicht getroffen");
        }
    }
    //Runde wird inkrementiert && AP werden wieder aufgefüllt
    void nextRound()
    {
        player1.GetComponent<PlayerComponent>().regenerateAP(); //Füllt AP von Spieler1 wieder auf
        player2.GetComponent<PlayerComponent>().regenerateAP(); //Füllt AP von Spieler2 wieder auf
        rounds++;
        Debug.Log("Runde: " + rounds);
    }

    //Liefer true, wenn Spieler1 am Zug
    public bool getPlayerTurn()
    {
        return isPlayer1;
    }

    //Legt fest, welcher Spieler am Zug ist
    public void setPlayerTurn()
    {
        AudioManager.playEndTurn();
        roundHalf++;
        if(roundHalf == 2)
        {
            nextRound();
            roundHalf = 0;
        }
        isPlayer1 = !isPlayer1;
        if(isPlayer1) //wenn Spieler 1 dran ist
        {


            UnitSelectionEvent.Send(unitListP1[0]);
            cam.setNewTarget(selected_Figurine);                 //Gibt der Kamera ein neues Target
            inputSystem input = player1.GetComponent<inputSystem>();
            input.enabled = true;
            player2.GetComponent<inputSystem>().enabled = false;
        }
        else
        {
            //To-Do: Mit UI verknüpfen 
            UnitSelectionEvent.Send(unitListP2[0]);                 
            cam.setNewTarget(selected_Figurine);                 //Gibt der Kamera ein neues Target
            inputSystem input = player2.GetComponent<inputSystem>();
            input.enabled = true;
            player1.GetComponent<inputSystem>().enabled = false;
        }

        //Setze Aktionen wieder frei
        if(isPlayer1)
            foreach(GameObject g in unitListP1)
                g.GetComponent<AttributeComponent>().canShoot = true;
        else
            foreach (GameObject g in unitListP2)
                g.GetComponent<AttributeComponent>().canShoot = true;
    }

    public void addUnit(int team)
    {

        //erzeuge einheit
        GameObject tmp = Instantiate(unitPrefab);

        if (team == 1)
        {
            tmp.transform.SetParent(player1.transform);
            unitListP1.Add( tmp );
            tmp.tag = "FigurSpieler1";
            tmp.GetComponent<AttributeComponent>().team = 1;
        }
        else if (team == 2)
        {
            tmp.transform.SetParent(player2.transform);
            unitListP2.Add(tmp );
            tmp.tag = "FigurSpieler2";
            tmp.GetComponent<AttributeComponent>().team = 2;
        }

        
        placeUnit(team, tmp);
        
    }


    public void activeUnitMark()
    {
        if (selected_Figurine != null)
        {
            activeUnitMarker.transform.position = selected_Figurine.transform.position;
            activeUnitMarker.transform.rotation = selected_Figurine.transform.rotation;
        }
        

    }


    public void placeUnit(int team, GameObject unit)
    {
        Vector2 posi = new Vector2(0,0);

        if (team == 1)
        {
            posi = plane.GetComponent<BattlefieldCreater>().startPostionsP1[unitListP1.Count];           
        }
        else if (team == 2)
        {
            posi = plane.GetComponent<BattlefieldCreater>().startPostionsP2[unitListP1.Count];
        }

        //setze map Coordinaten
        unit.GetComponent<ObjectSetter>().x = (int) posi.x;
        unit.GetComponent<ObjectSetter>().z = (int) posi.y;
        unit.GetComponent<AttributeComponent>().setCurrentCell((int)posi.x, (int)posi.y);



        //setze welt coordinaten
        float xBase = plane.transform.position.x -plane.transform.localScale.x *10 /2;
        float yBase = plane.transform.position.y;
        float zBase = 0;


        unit.transform.position = new Vector3(xBase + (int)posi.x + 0.5f, yBase + 0.52f, zBase - (int)posi.y - 0.5f );
        // tmp.transform.position = new Vector3(x,1.0f,z)



    }

    public void removeUnit(GameObject unit, int team)
    {

        //spiel zuende
        if(unitListP1.Count <= 1){
            Debug.Log("Spieler 2 gewinnt");
            ende();
        }else if(unitListP2.Count <= 1){
            Debug.Log("Spieler 1 gewinnt");
            ende();
        }



        if (team == 1)
        {
            //sterbende Figur ist aktuelle figur
            if (unit == selected_Figurine)
            {
                UnitSelectionEvent.Send(unitListP1[0]);
                activeUnitMark();
            }

            unitListP1.Remove(unit);
        }
        else if (team == 2)
        {
            //sterbende Figur ist aktuelle figur
            if (unit == selected_Figurine)
            {
                UnitSelectionEvent.Send(unitListP2[0]);
                activeUnitMark();
            }
            unitListP2.Remove(unit);
        }




    }

    public void ende()
    {
        Debug.Log("Ende");
    }

}
