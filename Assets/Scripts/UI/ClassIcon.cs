using UnityEngine;
using System.Collections;

public class ClassIcon : MonoBehaviour {

    public int width;
    public int height;
    public int figureOffset;
    public int barOffset;


    int baseX;
    int baseY;

    public GameObject unit;

    public Texture2D iconRiot;
    public Texture2D iconSoldier;
    public Texture2D iconSupport;

    public Enums.Prof prof;

    public Texture2D iconToShow;

    public int xOffset = 0;
    public int yOffset = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Icon at start " + iconToShow);
        unit = this.transform.gameObject;
        prof = unit.GetComponent<AttributeComponent>().profession;

        if (prof == Enums.Prof.Riot)
            iconToShow = iconRiot;
        if (prof == Enums.Prof.Soldier)
            iconToShow = iconSoldier;
        if (prof == Enums.Prof.Support)
            iconToShow = iconSupport;

        Debug.Log(iconToShow);
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void OnGUI()
    {

        if (Input.GetKey("tab") && iconToShow != null)
        {

            Debug.Log("FICK DICH");

            int healthIconWidth = GetComponentInParent<HealthBar>().width;
            int healthIconHeight = GetComponentInParent<HealthBar>().height;

            int hpBars = (unit.GetComponentInParent<AttributeComponent>().hp + 9) / 10;

            int xOffset = 0;
            int yOffset = 0;

            if (hpBars <= 5)
            {
                xOffset = (hpBars * (healthIconWidth + barOffset) / 2);
                yOffset = 0;
            }
            else
            {
                xOffset = (5 * (healthIconWidth + barOffset) / 2);
                yOffset = hpBars / 5 * (healthIconHeight + barOffset);
            }
            
             

            Vector3 worldPosition = unit.transform.position;
            worldPosition.y += figureOffset;
            Vector3 position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(worldPosition);

            /*
            //links neben healthbars
            GUI.DrawTexture(new Rect(position.x - xOffset - width, (Screen.height - position.y) - yOffset , width, height), iconToShow);
            */
            //links über healthbars
            Debug.Log("Class offset: " + xOffset + ", " + yOffset);
            GUI.DrawTexture(new Rect(position.x - xOffset , (Screen.height - position.y) - yOffset - healthIconHeight - 2*barOffset, width, height), iconToShow);
            /*
            //links über neben healthbars
            GUI.DrawTexture(new Rect(position.x - xOffset - width, (Screen.height - position.y) - yOffset - height, width, height), iconToShow);
            */
        }

    }
}
