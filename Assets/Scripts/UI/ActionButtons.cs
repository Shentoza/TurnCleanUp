using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour {

    UiManager uiM;

    public int bottomSpacing;
    public int width;
    public int height;
    public int spacing;

    //button icons
    public Texture2D Cancel;
    public Texture2D Move;
    public Texture2D Hit;
    public Texture2D Shoot;
    public Texture2D Reload;
    public Texture2D ChangeWeapon;
    public Texture2D Heal;
    public Texture2D Molotov;
    public Texture2D Grenade;
    public Texture2D Smoke;
    public Texture2D Teargas;

    public Texture2D activeSkillBackground;

    public int buttonsToDraw;

    public List<Enums.Actions> skills;
    int startPosX;

    
   

	// Use this for initialization
	void Start () {
        uiM = GetComponent<UiManager>();
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnGUI(){

      
        
        skills = uiM.getActiveUnitSkills();

        buttonsToDraw = skills.Count;
      
        startPosX = Screen.width/2 - (buttonsToDraw * width + (buttonsToDraw - 1) * spacing) / 2;

        for (int i = 0; i < buttonsToDraw; i++)
        {
            drawButton(i);
        //    Debug.Log("button " + i + "erstellt");
        }


        // tooltext label
        // GUI.Label(new Rect(Screen.width / 2, Screen.height - height - bottomSpacing + 5, 40, 40), GUI.tooltip);
        GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y, 50, 50), GUI.tooltip);
    }

    void drawButton(int i){
  
        int posX;
        if (i == 0)
        {
            posX = startPosX;
        }
        else
        {
            posX = startPosX + i * width + i * spacing;
        }
        int posY = Screen.height - height - bottomSpacing;


        
        
        if (skills[i] == Enums.Actions.Move)
        {

            if (uiM.activeSkill == Enums.Actions.Move)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }

            bool clicked1 = GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Move, "Move"));           
            if(clicked1)
            {
                
                uiM.move();
                Debug.Log("move");
            }
        }
        else if (skills[i] == Enums.Actions.Hit)
        {
            if (uiM.activeSkill == Enums.Actions.Hit)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked2 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Hit, "Hit")));
          
            if(clicked2)
            {
                uiM.hit();
                Debug.Log("hit");
            }
        }
        else if (skills[i] == Enums.Actions.Shoot){
            if (uiM.activeSkill == Enums.Actions.Shoot)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
        
            bool clicked3 =(GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Shoot, "Shoot")));
  
            if(clicked3)
            {
                uiM.shoot();
                Debug.Log("shoot");
            }
        }
        else if (skills[i] == Enums.Actions.Reload){
          /*  if (uiM.activeSkill == Enums.Actions.Reload)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
        */
            bool clicked4 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Reload, "Reload")));
       
            if(clicked4)
            {
                
                uiM.reload();
                Debug.Log("Reload");
            }
        }
        else if (skills[i] == Enums.Actions.ChangeWeapon){
            /*if (uiM.activeSkill == Enums.Actions.ChangeWeapon){
            
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
        */
            bool clicked5 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(ChangeWeapon, "Change Weapon")));
        
            if(clicked5)
            {
                uiM.changeWeapon();
                Debug.Log("change weapon");
            }
        }
        else if (skills[i] == Enums.Actions.Heal)
        {
            if (uiM.activeSkill == Enums.Actions.Heal)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked6 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Heal, "Heal")));
         
            if(clicked6)
            {
                uiM.heal();
                Debug.Log("heal");
            }
        }
        else if (skills[i] == Enums.Actions.Molotov)
        {
            if (uiM.activeSkill == Enums.Actions.Molotov)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked7 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Molotov, "Molotov")));
        
            if(clicked7)
            {
                uiM.molotov();
            }
        }
        else if (skills[i] == Enums.Actions.Grenade)
        {
            if (uiM.activeSkill == Enums.Actions.Grenade)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked8 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Grenade, "Grenade")));
    
            if(clicked8)
            {
                uiM.grenade();
                Debug.Log("Grenade");
            }
        }
        else if (skills[i] == Enums.Actions.Smoke)
        {
            if (uiM.activeSkill == Enums.Actions.Smoke)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked9 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Smoke, "Smoke")));
   
            if(clicked9)
            {
                uiM.smoke();
                Debug.Log("Smoke");
            }
        }
        else if (skills[i] == Enums.Actions.Teargas)
        {
            if (uiM.activeSkill == Enums.Actions.Teargas)
            {
                GUI.Label(new Rect(posX, posY, width, height), activeSkillBackground);
            }
            bool clicked10 = (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(Teargas, "Teargas")));
          
            if (clicked10)
            {
                uiM.teargas();
                Debug.Log("Teargas");
            }
        }

        


    }
}
