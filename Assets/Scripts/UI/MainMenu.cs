using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


    public Texture2D StartGameButton;
    public Texture2D OptionButtons;
    public Texture2D BackButton;
    public Texture2D ExitGameButton;

    public int buttonWidth = 100;
    public int buttonHeight = 50;

    enum States { StartState, OptionsState};
    States state;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
	}

   void OnGUI(){

       //main state
       if (state == States.StartState)
       {
           

           //StartButton
           if (GUI.Button(new Rect((int)(Screen.width / 2 - buttonWidth / 2), (int)(Screen.height / 4 - buttonHeight / 2), buttonWidth, buttonHeight), new GUIContent(StartGameButton, "Start")))
           {
                SceneManager.LoadScene("MPTestScene");
           }
           //OptionsButton
           if (GUI.Button(new Rect((int)(Screen.width / 2 - buttonWidth / 2), (int)(Screen.height /2 - buttonHeight / 2), buttonWidth, buttonHeight),  new GUIContent(OptionButtons,"Options")))
           {
               state = States.OptionsState;
           }
           //ExitButton
           if (GUI.Button(new Rect((int)(Screen.width / 2 - buttonWidth / 2), (int)(Screen.height /4 * 3 - buttonHeight / 2), buttonWidth, buttonHeight),  new GUIContent( ExitGameButton, "Exit")))
           {
               Application.Quit();
           }

           GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y, 50, 50), GUI.tooltip);
       }

       // option state
       else if( state == States.OptionsState){

           //back button
           if (GUI.Button(new Rect((int)(Screen.width / 2 - buttonWidth / 2), (int)(Screen.height /4 * 3 - buttonHeight / 2), buttonWidth, buttonHeight),  new GUIContent(BackButton, "Back" )) )
           {
               state = States.StartState;
           }
           GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y, 50, 50), GUI.tooltip);
       }



    }


}
