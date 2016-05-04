using UnityEngine;
using System.Collections;

public class APBars : MonoBehaviour {

    //ref object
    UiManager uiM;

    //AP Leisten
    public Texture2D frame;
    public Texture2D textureP1APt;
    public Texture2D textureP1APf;
    public Texture2D textureP2APt;
    public Texture2D textureP2APf;


    public int topSpacing;
    public int width;
    public int height;


	// Use this for initialization
	void Start () {
       uiM = GetComponent<UiManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}


    void OnGUI()
    {
        int baseX = Screen.width / 2;
        int frameWidth = uiM.maxAP * width * 2 + 7;

        GUI.DrawTexture(new Rect((baseX - 4 - uiM.maxAP * width), topSpacing , frameWidth, height + 4), frame);

        //player 1
        int offset = 2;

        for (int i = 0; i < uiM.player1AP; i++ ){
            GUI.DrawTexture(new Rect((baseX - 1 - width - offset), topSpacing + 2, width, height), textureP1APt);
            offset += width;
        }
        for (int i = uiM.player1AP; i < uiM.maxAP; i++)
        {
            GUI.DrawTexture(new Rect((baseX - 1 - width - offset), topSpacing + 2, width, height), textureP1APf);
            offset += width;
        }
        


        //player 2
        offset = 2;

        for (int i = 0; i < uiM.player2AP; i++)
        {
            GUI.DrawTexture(new Rect(baseX + 1 + offset, topSpacing + 2, width, height), textureP2APt);
            offset += width;
        }
        for (int i = uiM.player2AP; i < uiM.maxAP; i++)
        {
            GUI.DrawTexture(new Rect(baseX + 1 + offset, topSpacing + 2, width, height), textureP2APf);
            offset += width;
        }
        
    }


}
