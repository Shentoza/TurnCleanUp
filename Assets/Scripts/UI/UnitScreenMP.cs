using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;



public class UnitScreenMP : MonoBehaviour {

    public int p1UnitCap=0;
    public int p2UnitCap=0;


    //ui positioning and size
    public int unitIconWidth = 75;
    public int unitIconHeight = 75;
    public int unitListXAnkerP1 = 50;
    int unitListXAnkerP2 = -1;
    public int unitListYAnker = 50;  
    public int borderWidth = 2;



    public float dropdownBaseX = 0.3f;
    public int dropdownOptionSize = 50;
    public int buttonYOffset = 100;





    int unitCountP1=0;
    int unitCountP2=0;

    public ManagerSystem manager;


    public Texture2D backgroundTexture;
    public Texture2D notPickingTexture;
    public Texture2D unitListBackground;
    public Texture2D unitBackground;
    public Texture2D newUnitButton;


    // für initialisierungsreihenfolge siehe Enums.cs
    public List<Texture2D> pWeapons, sWeapons, util;
  
    public Texture2D riotTex;
    public Texture2D soldierTex;
   // public Texture2D hGTex;
    public Texture2D supportTex;
   // public Texture2D sniperTex;

    Vector4 equip = new Vector4(0,0,0,0);


    int xAnkerP1;
    int yAnkerP1;

    //dropdown
    int c = 0;
    bool dp1, dp2, dp3, dp4;

    //picking abfolge
    bool player1Picking = true;

    bool done = false;

   	// Use this for initialization
	void Start () {
         
    unitListXAnkerP2 = Screen.width - unitListXAnkerP1 - unitIconWidth - 2 * borderWidth;
    p1UnitCap = manager.p1UnitCap;
    p2UnitCap = manager.p2UnitCap;
    GameObject.Find("Main Camera").GetComponent<CameraRotationScript>().enabled = false;   
        
	}
	
	// Update is called once per frame
	void Update () {

        if (!done)
        {
            unitListXAnkerP2 = Screen.width - unitListXAnkerP1 - unitIconWidth - 2 * borderWidth;

            if (unitCountP1 > unitCountP2 && unitCountP2 < p2UnitCap)
            {
                player1Picking = false;
            }
            else
            {
                player1Picking = true;
            }



            dropdownUpdate();

            if (unitCountP1 >= p1UnitCap && unitCountP2 >= p2UnitCap)
            {
                //pickingphase beenden 
                //übergang zum gameplay
                done = true;
                manager.startGame();
                Destroy(this);
            }
        }
	}


    void  p1Pick(){
        AudioManager.playMainClick();

        if (player1Picking)
        {
            setChar(1, equip);
            equip = new Vector4(0, 0, 0, 0);
            unitCountP1++;
        }
        
    }

    void p2Pick( Enums.Prof i)
    {
        AudioManager.playMainClick();

        if (!player1Picking)
        {
            setChar(2, i);
            unitCountP2++;
        }
        
    }

    void OnGUI()
    {
        if (!done)
        {
            //erstelle hintergrund
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);          

            //rebellen
            drawP1();

            //staat
            drawP2();


            //picking info   
            GUIStyle gs = new GUIStyle();
            gs.fixedHeight = Screen.height;
            gs.fixedWidth = 0;
            gs.fixedHeight = 0;
            gs.stretchHeight = true;
            gs.stretchWidth = true;

            if (player1Picking)
            {
                GUI.Label(new Rect(Screen.width / 2 + 1, 0, Screen.width / 2, Screen.height+200), notPickingTexture,gs);
            }
            else
            {
                GUI.Label(new Rect(0, 0, Screen.width / 2, Screen.height+200), notPickingTexture,gs);
            }
          

            //Tooltip label
            GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y, 150, 150), GUI.tooltip);

        }
    }


    //rebellen
    void drawP1()
    {

        //einheitenlisten hintergrund
        int unitListXAnker = unitListXAnkerP1;     
        GUI.DrawTexture(new Rect(unitListXAnker, unitListYAnker, unitIconWidth + 2 * borderWidth, (int)(Screen.height * 0.8f)), unitListBackground);


        //einheitenliste
        xAnkerP1 = unitListXAnker + borderWidth;
        yAnkerP1 = unitListYAnker;
        drawUnitList();


        //add unit button
        if (player1Picking)
        {
            bool p1PickButton = GUI.Button(new Rect(unitListXAnker, unitListYAnker + (int)(Screen.height * 0.8f) + 5, unitIconWidth, unitIconHeight), new GUIContent(newUnitButton, "Order Now"));
            if(p1PickButton){
                if ((Enums.PrimaryWeapons)equip.x != Enums.PrimaryWeapons.None)
                {
                    p1Pick();
                }
                
            }
        }

        //ausrüstungsbuttons auswahlbuttons
        drawEquipButtons();
        drawDropdown();

    
    }


    void drawDropdown()
    {
        int xBase = 0;
        int yBase = 0;
        int draw = 0;

        

        if (dp1 == true && player1Picking)
        {
            xBase = (int)(Screen.width * dropdownBaseX) + unitIconWidth/2;
            yBase = unitListYAnker + unitIconHeight + 2;
            draw = 1;
        }
        else if (dp2 == true && player1Picking)
        {
            xBase = (int)(Screen.width * dropdownBaseX) + unitIconWidth / 2;
            yBase = unitListYAnker + buttonYOffset + unitIconHeight + 2; ;
            draw = 2;
        }
        else if (dp3 == true && player1Picking)
        {
            xBase = (int)(Screen.width * dropdownBaseX) + unitIconWidth / 2;
            yBase = unitListYAnker + 2*buttonYOffset + unitIconHeight + 2;
            draw = 3;
        }
        else if (dp4 == true && player1Picking)
        {
            xBase = (int)(Screen.width * dropdownBaseX) + unitIconWidth / 2;
            yBase = yBase = unitListYAnker + 3 * buttonYOffset + unitIconHeight + 2;
            draw = 3;
        }

        //kein dropdown
        if (draw == 0)
        {
        }
        //primärwaffen dropdown
        else if (draw == 1)
        {
            xBase -= (Enum.GetNames(typeof(Enums.PrimaryWeapons)).Length * dropdownOptionSize / 2);
            for (int i = 0; i < Enum.GetNames(typeof(Enums.PrimaryWeapons)).Length; i++)
            {
                if (GUI.Button(new Rect(xBase + i * dropdownOptionSize, yBase, dropdownOptionSize, dropdownOptionSize), new GUIContent(pWeapons[i], ((Enums.PrimaryWeapons)i).ToString())))
                {
                    AudioManager.playSecondClick();

                    equip.x = i;
                    dp1 = false;             
                }
            }
        }
        //sekundärwaffen dropdown
        else if (draw == 2)
        {
            xBase -= (Enum.GetNames(typeof(Enums.SecondaryWeapons)).Length * dropdownOptionSize / 2);
            for (int i = 0; i < Enum.GetNames(typeof(Enums.SecondaryWeapons)).Length; i++)
            {
                if (GUI.Button(new Rect(xBase + i * dropdownOptionSize, yBase, dropdownOptionSize, dropdownOptionSize), new GUIContent(sWeapons[i], ((Enums.SecondaryWeapons)i).ToString())))
                {
                    AudioManager.playSecondClick();

                    equip.y = i;
                    dp2 = false;
                    

                }
            }
        }
        //untility dropdown
        else if (draw == 3)
        {
            xBase -= (Enum.GetNames(typeof(Enums.Equipment)).Length * dropdownOptionSize / 2);
            for (int i = 0; i < Enum.GetNames(typeof(Enums.Equipment)).Length; i++)
            {
                if (GUI.Button(new Rect(xBase + i * dropdownOptionSize, yBase, dropdownOptionSize, dropdownOptionSize),new GUIContent( util[i], ((Enums.Equipment)i).ToString())))
                {
                    AudioManager.playSecondClick();

                    if (dp3)
                    {
                        equip.z = i;
                        dp3 = false;
                    }
                    else if (dp4)
                    {
                        equip.w = i;
                        dp4 = false;
                    }

                }
            }
        }

    }

    void dropdownUpdate(){
        if ((dp1 | dp2 | dp3 | dp4) & Input.GetMouseButtonDown(0))
        {
            c = 8;
        }
        if (c == 0)
        {
            dp1 = false;
            dp2 = false;
            dp3 = false;
            dp4 = false;
        }
        c = Math.Max(-1, --c);
    }

    void drawEquipButtons()
    {



        if (GUI.Button(new Rect((int)(Screen.width * dropdownBaseX), unitListYAnker, 75, 75), new GUIContent(pWeapons[(int)equip.x], ((Enums.PrimaryWeapons)equip.x).ToString())))
        {
            AudioManager.playMainClick();

            dp1 = true;
        }
        if (GUI.Button(new Rect((int)(Screen.width * dropdownBaseX), unitListYAnker + buttonYOffset, 75, 75), new GUIContent( sWeapons[(int)equip.y], ((Enums.SecondaryWeapons)equip.y).ToString())))
        {
            AudioManager.playMainClick();

            dp2 = true;
        }
        if (GUI.Button(new Rect((int)(Screen.width * dropdownBaseX), unitListYAnker + 2 * buttonYOffset, 75, 75), new GUIContent( util[(int)equip.z], ((Enums.Equipment)equip.z).ToString())))
        {
            AudioManager.playMainClick();

            dp3 = true;
        }
        if (GUI.Button(new Rect((int)(Screen.width * dropdownBaseX), unitListYAnker + 3 * buttonYOffset, 75, 75), new GUIContent( util[(int)equip.w], ((Enums.Equipment)equip.w).ToString())))
        {
            AudioManager.playMainClick();

            dp4 = true;
        }
        
       
    }

    void drawUnitList()
    {
        for (int i = 0; i < unitCountP1; i++)
        {

            yAnkerP1 += borderWidth;

            //draw einheit
            drawUnitIcon(i, xAnkerP1, yAnkerP1);

            yAnkerP1 += unitIconHeight + borderWidth;

        }
    }

    void drawUnitIcon(int unitID, int xPos, int yPos)
    {

        GUIStyle gs = new GUIStyle();

        if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unitBackground,gs))
        {
            /*   if (Event.current.button == 0)
               {
                   activeUnit = unitID;
               }
               else if (Event.current.button == 1)
               {
                   unitsList.RemoveAt(unitID);
               }
             * */
        }

        //ausrüstung
        GUI.DrawTexture(new Rect(4 + xPos, yPos + unitIconHeight / 2 - unitIconWidth / 10, unitIconWidth / 5, unitIconWidth / 5), pWeapons[(int)(manager.unitListP1[unitID].GetComponent<AttributeComponent>().items.primaryWeaponType)]);
        GUI.DrawTexture(new Rect((4 + xPos + unitIconWidth / 5 + unitIconWidth / 5 / 6), yPos + unitIconHeight / 2 - unitIconWidth / 10, unitIconWidth / 5, unitIconWidth / 5), sWeapons[(int)(manager.unitListP1[unitID].GetComponent<AttributeComponent>().items.secondaryWeaponType)]);
        GUI.DrawTexture(new Rect((4 + xPos + 2 * (unitIconWidth / 5 + unitIconWidth / 5 / 6)), yPos + unitIconHeight / 2 - unitIconWidth / 10, unitIconWidth / 5, unitIconWidth / 5), util[(int)(manager.unitListP1[unitID].GetComponent<AttributeComponent>().items.utility1)]);
        GUI.DrawTexture(new Rect((4 + xPos + 3 * (unitIconWidth / 5 + unitIconWidth / 5 / 6)), yPos + unitIconHeight / 2 - unitIconWidth / 10, unitIconWidth / 5, unitIconWidth / 5), util[(int)(manager.unitListP1[unitID].GetComponent<AttributeComponent>().items.utility2)]);
    }




    //staats einheiten 
    void drawP2()
    {
        int unitListXAnker = unitListXAnkerP2;

        //einheitenlisten hintergrund
        GUI.DrawTexture(new Rect(unitListXAnker,unitListYAnker, unitIconWidth+2*borderWidth, (int)(Screen.height*0.8f)), unitListBackground);


        //einheitenliste
        int xAnker = unitListXAnker + borderWidth;
        int yAnker = unitListYAnker;

      

        for (int i = 0; i < unitCountP2; i++ )
        {
           
            yAnker += borderWidth;

            //draw einheit
            drawP2UnitIcon(i, xAnker, yAnker);

            yAnker += unitIconHeight + borderWidth;

        }

     

        //einheiten auswahlbuttons
        //riot
        if (GUI.Button(new Rect((int)(Screen.width * 0.55), unitListYAnker, unitIconWidth, unitIconHeight), new GUIContent(riotTex, "Riot")))
        {
            p2Pick(Enums.Prof.Riot);
        }

        if (GUI.Button(new Rect((int)(Screen.width * 0.55), unitListYAnker + buttonYOffset, unitIconWidth, unitIconHeight), new GUIContent(soldierTex, "Soldier")))
        {
            if (!player1Picking)
            {
                p2Pick(Enums.Prof.Soldier);
            }
  
        }
    /*    if (GUI.Button(new Rect((int)(Screen.width * 0.55), unitListYAnker + buttonYOffset, unitIconWidth, unitIconHeight), new GUIContent(hGTex, "HeavyGunner")))
        {
            if (!player1Picking)
            {
                p2Pick(Enums.Prof.HeavyGunner);
            }
   
        }
     * */
        if (GUI.Button(new Rect((int)(Screen.width * 0.55), unitListYAnker + 2* buttonYOffset, unitIconWidth, unitIconHeight), new GUIContent(supportTex, "Support")))
        {
            if (!player1Picking)
            {
                p2Pick(Enums.Prof.Support);
            }
        }
        /*
        if (GUI.Button(new Rect((int)(Screen.width * 0.55), unitListYAnker + 2 * buttonYOffset, unitIconWidth, unitIconHeight), new GUIContent(sniperTex, "Sniper")))
        {
            if (!player1Picking)
            {
                p2Pick(Enums.Prof.Sniper);
            }
        }
        */
    }


    void drawP2UnitIcon(int unitID, int xPos, int yPos )
    {

        if (manager.unitListP2[unitID].GetComponent<AttributeComponent>().profession == Enums.Prof.Riot)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), riotTex))
            {
              /*  if (Event.current.button == 0){
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                   unitsList.RemoveAt(unitID);
                }
               */
            }
        }
        else if (manager.unitListP2[unitID].GetComponent<AttributeComponent>().profession == Enums.Prof.Soldier)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), soldierTex))
            {
             
            }
        }
      
        else if (manager.unitListP2[unitID].GetComponent<AttributeComponent>().profession == Enums.Prof.Support)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), supportTex))
            {
               
            }
        }
            /*
        else if (manager.unitListP2[unitID].GetComponent<AttributeComponent>().profession == Enums.Prof.HeavyGunner)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), hGTex))
            {

            }

        }
        else if (manager.unitListP2[unitID].GetComponent<AttributeComponent>().profession == Enums.Prof.Sniper)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), sniperTex))
            {

            }
        }
             * */
    }


    void setChar(int team, Vector4 equip)
    {
        manager.addUnit(team);
        manager.unitListP1[unitCountP1].GetComponent<AttributeComponent>().setEquip(equip);
    }


    void setChar(int team, Enums.Prof i)
    {
        manager.addUnit(team);
        manager.unitListP2[unitCountP2].GetComponent<AttributeComponent>().setProf((int)i);
    }





}



   