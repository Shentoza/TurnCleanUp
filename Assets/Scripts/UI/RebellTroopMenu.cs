using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RebellTroopMenu : MonoBehaviour {

    //dummy werte in drawDropdown und drawEquipButtons



    public int unitIconWidth;
    public int unitIconHeight;
    public int unitListXAnker = 50;
    public int unitListYAnker = 50;
    public int borderWidth = 2;


    public Texture2D backgroundTexture;
    public Texture2D unitListBackground;
    public Texture2D activeUnitFrame;

    public Texture2D newUnitButton;


    public List<Texture2D> weapons, util;



    int activeUnit=0;
    bool klicked, dp1, dp2, dp3, dp4;
    int xAnker;
    int yAnker;

    //dummys
    // prim. weapon, sec weapon, util1, util2
    List<Vector4> unitsList = new List<Vector4>();
   

	// Use this for initialization
	void Start () {

        //get units -> units[];
        //test
        unitsList.Add(new Vector4(0,0,0,0));
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {

        //erstelle hintergrund
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

        //einheitenlisten hintergrund
        GUI.DrawTexture(new Rect(unitListXAnker,unitListYAnker, unitIconWidth+2*borderWidth, (int)(Screen.height*0.8f)), unitListBackground);


        //einheitenliste
        xAnker = unitListXAnker + borderWidth;
        yAnker = unitListYAnker;
        drawUnitList();
        

        //add unit button
        if (GUI.Button(new Rect(unitListXAnker, unitListYAnker+(int)(Screen.height * 0.8f) + 5, unitIconWidth, unitIconHeight), newUnitButton))
        {       
            unitsList.Add(new Vector4(0, 0, 0, 0));
            activeUnit = unitsList.Count-1;
            
        }
        

        //ausrüstungsbuttons auswahlbuttons
        drawEquipButtons();
        drawDropdown();

    
    }

    void drawDropdown()
    {
        int xBase=0;
        int yBase=0;
        int draw= 0;

        int optionSize=50;

        if (dp1 == true)
        {
            xBase = 502;
            yBase = 50;
            draw = 1;
        } else if (dp2 == true)
        {
            xBase = 502;
            yBase = 175;
            draw = 1;
        } else if (dp3 == true)
        {
            xBase = 502;
            yBase = 300;
            draw = 2;
        } else if (dp4 == true)
        {
            xBase = 502;
            yBase = 425;
            draw = 2;
        }

        if (draw == 0){
        }
        else if (draw == 1)
        {
            yBase -= (weapons.Count * optionSize / 2);
            for (int i = 0; i < weapons.Count; i++)
            {
                if (GUI.Button(new Rect(xBase, yBase+i*optionSize, optionSize, optionSize), weapons[i]))
                {
                    if(dp1){
                        unitsList[activeUnit] = new Vector4(i, unitsList[activeUnit].y, unitsList[activeUnit].z, unitsList[activeUnit].w);
                        dp1 = false;
                    }
                    else if (dp2)
                    {
                        unitsList[activeUnit] = new Vector4(unitsList[activeUnit].x, i, unitsList[activeUnit].z, unitsList[activeUnit].w);
                        dp2 = false;
                    }
                    
                }
            }
        }
        else if (draw == 2)
        {
            yBase -= (weapons.Count * optionSize / 2);
            for (int i = 0; i < util.Count; i++)
            {
                if (GUI.Button(new Rect(xBase, yBase + i * optionSize, optionSize, optionSize), util[i]))
                {
                    if (dp3)
                    {
                        unitsList[activeUnit] = new Vector4(unitsList[activeUnit].x, unitsList[activeUnit].y, i, unitsList[activeUnit].w);
                        dp3 = false;
                    }
                    else if (dp4)
                    {
                        unitsList[activeUnit] = new Vector4(unitsList[activeUnit].x, unitsList[activeUnit].y, unitsList[activeUnit].z, i);
                        dp4 = false;
                    }

                }
            }
        }

    }


    void drawEquipButtons()
    {
        if (unitsList.Count == 0)
        {
            if (GUI.Button(new Rect(400, 50, 100, 100), weapons[0]))
            {
                dp1 = true; 
            }
            if (GUI.Button(new Rect(400, 175, 100, 100), weapons[0]))
            {
                dp2 = true; 
            }
            if (GUI.Button(new Rect(400, 300, 100, 100), util[0]))
            {
                dp3 = true; 
            }
            if (GUI.Button(new Rect(400, 425, 100, 100), util[0]))
            {
                dp4 = true; 
            }
        }
        else
        {
            if (GUI.Button(new Rect(400, 50, 100, 100), weapons[(int)(unitsList[activeUnit].x)]))
            {
                dp1 = true; ;
            }
            if (GUI.Button(new Rect(400, 175, 100, 100), weapons[(int)(unitsList[activeUnit].y)]))
            {
                dp2 = true; ;
            }
            if (GUI.Button(new Rect(400, 300, 100, 100), util[(int)(unitsList[activeUnit].z)]))
            {
                dp3 = true; ;
            }
            if (GUI.Button(new Rect(400, 425, 100, 100), util[(int)(unitsList[activeUnit].w)]))
            {
                dp4 = true; ;
            }
        }
    }


    void drawUnitIcon(int unitID, int xPos, int yPos )
    {          
        if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unitListBackground))
        {
            if (Event.current.button == 0){
                activeUnit = unitID;
            }
            else if (Event.current.button == 1)
            {
                unitsList.RemoveAt(unitID);
            }
        }      
        
        //ausrüstung
        GUI.DrawTexture(new Rect(2 + xPos, yPos + 10, unitIconWidth / 5, unitIconWidth / 5), weapons[(int)(unitsList[unitID].x)]);
        GUI.DrawTexture(new Rect((2 + xPos + unitIconWidth / 5 + unitIconWidth / 5 / 6), yPos + 10, unitIconWidth / 5, unitIconWidth / 5), weapons[(int)(unitsList[unitID].y)]);
        GUI.DrawTexture(new Rect((2 + xPos + 2 * (unitIconWidth / 5 + unitIconWidth / 5 / 6)), yPos + 10, unitIconWidth / 5, unitIconWidth / 5), util[(int)(unitsList[unitID].z)]);
        GUI.DrawTexture(new Rect((2 + xPos + 3 * (unitIconWidth / 5 + unitIconWidth / 5 / 6)), yPos + 10, unitIconWidth / 5, unitIconWidth / 5), util[(int)(unitsList[unitID].w)]);
    }



    void drawUnitList()
    {
        for (int i = 0; i < unitsList.Count; i++)
        {
            //ist einheit ausgewählt
            if (i == activeUnit)
            {

                GUI.DrawTexture(new Rect(unitListXAnker, yAnker, unitIconWidth + 2 * borderWidth, unitIconHeight + 2 * borderWidth), activeUnitFrame);

            }
            yAnker += borderWidth;

            //draw einheit
            drawUnitIcon(i, xAnker, yAnker);

            yAnker += unitIconHeight + borderWidth;

        }
    }








}
