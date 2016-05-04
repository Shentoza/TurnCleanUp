using UnityEngine;
using System.Collections;

public class PlayerComponent : MonoBehaviour {


    public Texture2D teamColor;

    GameObject[] figurines = new GameObject[3]; //Alle Figuren über die ein Spieler verfügt
    public int actionPoints = 0; //Anzahl an verfügbaren Aktionspunkten
    int maxAP; //Maxcap für AP

	// Use this for initialization
	void Start () {
        if (this.gameObject.name == "Player1")
            maxAP = (figurines.Length + 2) * 2;
        else if (this.gameObject.name == "Player2")
            maxAP = (figurines.Length + 4) * 2;

        regenerateAP();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Löscht Spielfigur
    public void removeFigurine(GameObject figurine)
    {
        for(int i = 0; i < figurines.Length; i++)
        {
            if(figurines[i] == figurine)
            {
                GameObject.Destroy(figurine);
                break;
            }
        }
    }

    //Füllt die AP für den Spieler wieder auf
    public void regenerateAP()
    {
        if (this.gameObject.name == "Player1")
        {
            if (actionPoints + figurines.Length + 2 > maxAP)
                actionPoints = maxAP;
            else
            {
                actionPoints = actionPoints + figurines.Length + 2;
            }
            

        }
        if(this.gameObject.name == "Player2")
        {
            if (actionPoints + figurines.Length + 4 > maxAP)
                actionPoints = maxAP;
            else
                actionPoints = actionPoints + figurines.Length + 4;
        }
    }

    public void useAP()
    {
        actionPoints--;
    }
}
