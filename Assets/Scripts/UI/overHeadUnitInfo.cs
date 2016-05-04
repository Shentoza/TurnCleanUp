using UnityEngine;
using System.Collections;

public class overHeadUnitInfo : MonoBehaviour {


    public int healthIconWidth;
    public int healthIconHeight;

    public int classIconWidth;
    public int classIconHeight;

    public int ammoIconWidth;
    public int ammoIconHeight;

    public int figureScreenYOffset;
    public int figureWorldYOffset;
    public int barOffset;

    public Texture2D hpTexture;
    public Texture2D iconRiot;
    public Texture2D iconSoldier;
    public Texture2D iconSupport;
    public Texture2D ammoIcon;




    public UiManager uim;
    public GameObject unit;
    public Enums.Prof prof;
    public Texture2D iconToShow;
    public int xOffset = 0;
    public int yOffset = 0;


    int hpBars;
    int baseX;
    int baseY;

    bool active = false;

    // Use this for initialization
    void Start()
    {
  
        //bestimme zugehöriges gameobject
        unit = this.transform.gameObject;

        //bestimme klasse der einheit
        prof = unit.GetComponent<AttributeComponent>().profession;

        // bestimme health icon 
        hpTexture = unit.GetComponentInParent<PlayerComponent>().teamColor;


        //bestimme anzuzeigende klasse
        if (prof == Enums.Prof.Riot)
            iconToShow = iconRiot;
        if (prof == Enums.Prof.Soldier)
            iconToShow = iconSoldier;
        if (prof == Enums.Prof.Support)
            iconToShow = iconSupport;

       // Debug.Log(iconToShow);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("UiManager(Clone)"))
        {
            
            uim = GameObject.Find("UiManager(Clone)").GetComponent<UiManager>();
            active = true;
        }


        hpBars = (unit.GetComponent<AttributeComponent>().hp + 9) / 10;
    }


    void OnGUI()
    {

       if(active && (Input.GetKey("tab") | uim.input.angriffAusgewaehlt )){


           int xOffset = (5 * (healthIconWidth + barOffset) / 2);
            int yOffset = 0;
            bool showAmmo = false;

           //bestimme offset um anzahl der hp icon reihen zu berücksichtigen
            if (hpBars > 5)
            {          
                yOffset = hpBars / 5 * (healthIconHeight + barOffset);
            }


           //wenn einheit schusswaffe ausgerüstet hat
           if(unit.GetComponent<InventoryComponent>().isPrimary){
               if(unit.GetComponent<InventoryComponent>().primaryWeaponType == Enums.PrimaryWeapons.AssaultRifle){
                   showAmmo = true;
               }
           }
           else
           {
               if (unit.GetComponent<InventoryComponent>().secondaryWeaponType == Enums.SecondaryWeapons.Pistol)
               {
                   showAmmo = true;
               }
           }
           //passe offset für munitionsanzeige an
           if (showAmmo)
           {
              
               yOffset += ammoIconHeight + barOffset;
           }


           //bestimme position über der figur
            Vector3 worldPosition = unit.transform.position;
            worldPosition.y += figureWorldYOffset;

           //umwandlung in screen coordinaten
            Vector3 position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(worldPosition);
            position.y = (Screen.height - position.y - figureScreenYOffset);


            //zeichne class icon links über healthbars    
            if (iconToShow != null)
            {
                GUI.DrawTexture(new Rect(position.x - xOffset, position.y - yOffset - classIconHeight - barOffset, classIconWidth, classIconHeight), iconToShow);
                // Debug.Log("Class offset: " + xOffset + ", " + yOffset);
            }


           //zeichne healthbars
            yOffset = yOffset + (healthIconHeight + barOffset);
            for (int i = 0; i < hpBars; i++)
            {

                if (i % 5 == 0)
                {

                    yOffset = yOffset - (healthIconHeight + barOffset);
                }

                GUI.DrawTexture(new Rect(position.x - xOffset + i % 5 * (healthIconWidth + barOffset), position.y - yOffset, healthIconWidth, healthIconHeight), hpTexture);
                // Debug.Log("positionX: " + position.x + "position.y: " + position.y);
            }
         
           //zeichne ammo icons
            yOffset = yOffset - (healthIconHeight + barOffset);
            if (showAmmo)
            {
                int ammo = 0;

                if(unit.GetComponent<InventoryComponent>().isPrimary){
                    ammo = unit.GetComponent<InventoryComponent>().primary.currentBulletsInMagazine;
                }else{
                    ammo = unit.GetComponent<InventoryComponent>().secondary.currentBulletsInMagazine;
                }

                for (int i = 0; i < ammo; i++ )
                {
                    GUI.DrawTexture(new Rect(position.x - xOffset + i * (ammoIconWidth + barOffset),  position.y - yOffset, ammoIconWidth, ammoIconHeight), ammoIcon);
                }
            }

        }

    }
}
