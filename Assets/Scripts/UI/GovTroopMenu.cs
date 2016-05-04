using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GovTroopMenu : MonoBehaviour {

    public int unitIconWidth;
    public int unitIconHeight;
    public int unitListXAnker = 50;
    public int unitListYAnker = 50;
    public int borderWidth = 2;

    public Texture2D backgroundTexture;

    public Texture2D unitListBackground;
    public Texture2D activeUnitFrame;

    public Texture2D noUnit;
    public Texture2D unit1;
    public Texture2D unit2;
    public Texture2D unit3;
    public Texture2D unit4;


    int activeUnit;

    //dummys
    // 0 = keine einheit; 1 = streifenpolizist.....
    List<int> unitsList = new List<int>();
   

	// Use this for initialization
	void Start () {

        //get units -> units[];
        
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
        int xAnker = unitListXAnker + borderWidth;
        int yAnker = unitListYAnker;


        for (int i = 0; i < unitsList.Count; i++ )
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


        //einheiten auswahlbuttons
        if (GUI.Button(new Rect( (int)(Screen.width * 0.4), (int)(Screen.height * 0.4), 150, 100), unit1))
        {
            unitsList.Add(1);
            activeUnit = (activeUnit+1) % unitsList.Count;
        }
        if (GUI.Button(new Rect((int)(Screen.width * 0.6), (int)(Screen.height * 0.4), 150, 100), unit2))
        {
            unitsList.Add(2);
            activeUnit = (activeUnit + 1) % unitsList.Count;
        }
        if (GUI.Button(new Rect((int)(Screen.width * 0.4), (int)(Screen.height * 0.6), 150, 100), unit3))
        {
            unitsList.Add(3);
            activeUnit = (activeUnit + 1) % unitsList.Count;
        }
        if (GUI.Button(new Rect((int)(Screen.width * 0.6), (int)(Screen.height * 0.6), 150, 100), unit4))
        {
            unitsList.Add(4);
            activeUnit = (activeUnit + 1) % unitsList.Count;
        }


    }


    void drawUnitIcon(int unitID, int xPos, int yPos )
    {
        if (unitsList[unitID] == 0)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), noUnit))
            {
                if (Event.current.button == 0)
                {
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                    unitsList.RemoveAt(unitID);
                }
            }
        }
        else if (unitsList[unitID] == 1)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unit1))
            {
                if (Event.current.button == 0){
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                   unitsList.RemoveAt(unitID);
                }
            }
        }
        else if (unitsList[unitID] == 2)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unit2))
            {
                if (Event.current.button == 0)
                {
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                    unitsList.RemoveAt(unitID);
                }
            }
        }
        else if (unitsList[unitID] == 3)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unit3))
            {
                if (Event.current.button == 0)
                {
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                    unitsList.RemoveAt(unitID);
                }
            }

        }
        else if (unitsList[unitID] == 4)
        {
            if (GUI.Button(new Rect(xPos, yPos, unitIconWidth, unitIconHeight), unit4))
            {
                if (Event.current.button == 0)
                {
                    activeUnit = unitID;
                }
                else if (Event.current.button == 1)
                {
                    unitsList.RemoveAt(unitID);
                }
            }
        }
    }

}
