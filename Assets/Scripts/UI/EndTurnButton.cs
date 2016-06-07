using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class EndTurnButton : MonoBehaviour {

    //ref object
    UiManager uiM;

   
    //end Turn button
    public Texture2D iconPlayer1EndTurn;
    public Texture2D iconPlayer2EndTurn;


    public int width;
    public int height;


	// Use this for initialization
	void Start () {
        uiM = GetComponent<UiManager>();
	}
	

    void OnGUI()
    {

        if (uiM.isPlayer1)
        {
            if (GUI.Button(new Rect(Screen.width - width, Screen.height - height, width, height), new GUIContent(iconPlayer1EndTurn, "End Turn"), uiM.getStyle()))
            {
                EndturnEvent.Send(false);
            }
        }
        else if (GUI.Button(new Rect(Screen.width - width, Screen.height - height, width, height), new GUIContent(iconPlayer2EndTurn, "End Turn"), uiM.getStyle()))
        {
            EndturnEvent.Send(true);
        }
        GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y, 50, 50), GUI.tooltip);
    }
}
