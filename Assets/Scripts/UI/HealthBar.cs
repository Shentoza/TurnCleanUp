using UnityEngine;
using System.Collections;



public class HealthBar : MonoBehaviour {

    public int width;
    public int height;
    public int figureOffset;
    public int barOffset;
    public UiManager uim;

    int baseX;
    int baseY;   
    public GameObject unit;

    public Texture2D hpTexture;
    int hpBars;


	// Use this for initialization
	void Start () {
        unit = this.transform.gameObject;
        hpTexture = unit.GetComponentInParent<PlayerComponent>().teamColor;
        
    }
	

	// Update is called once per frame
	void Update () {

        if (GameObject.Find("UiManager(Clone)"))
        {
            uim = GameObject.Find("UiManager(Clone)").GetComponent<UiManager>();
        }


        hpBars = (unit.GetComponent<AttributeComponent>().hp+9 ) /10;

	}


    void OnGUI()
    {

       // Debug.Log("FICK");
        if(uim && (Input.GetKey("tab") | uim.input.angriffAusgewaehlt )){

            

            int xOffset = 0;
            int yOffset = 0;

            if(hpBars <= 5){
                xOffset = (hpBars * (width+barOffset) / 2);
                yOffset = 0;
            }
            else
            {
                xOffset = (5 * (width + barOffset) / 2);
                yOffset = hpBars / 5 * (height + barOffset);
            }
            

            //bestimme hp leisten position
            Vector3 worldPosition = unit.transform.position;
            worldPosition.y += figureOffset; 
            Vector3 position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint (worldPosition);

            Debug.Log("Health offset: " + xOffset + ", " + yOffset);

            for (int i = 0; i < hpBars; i++ )
            {

                if (i % 5 == 0)
                {
                   
                    yOffset = yOffset - (height + barOffset);
                }

                GUI.DrawTexture(new Rect(position.x - xOffset + i%5*(width+barOffset), (Screen.height - position.y) - yOffset, width, height), hpTexture);
               // Debug.Log("positionX: " + position.x + "position.y: " + position.y);
            }
            
           

        }

    }

}
