using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public Cell upperNeighbour;
	public Cell leftNeighbour;
	public Cell rightNeighbour;
	public Cell lowerNeighbour;
    public ArrayList neighbours = new ArrayList();

    public Cell dij_Vorgaenger;
    public int dij_KnotenKosten = 1;
    public int dij_GesamtKosten = int.MaxValue;
    public enum dij_Zustand { DIJ_UNBESUCHT, DIJ_ENTDECKT, DIJ_ABGESCHLOSSEN };
    public dij_Zustand dij_ZellZustand;
	public GameObject objectOnCell;

	public bool isOccupied = false;
	public bool setOnFire = false;
	public bool smoked = false;
    public bool setOnGas = false;

	public bool keineDeckung;
	public bool niedrigeDeckung;
	public bool hoheDeckung;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	 
	}

    

	public void setNeighbours(GameObject u, GameObject le, GameObject r, GameObject lo)
	{
        if (u != null)
        {
            upperNeighbour = (Cell)u.GetComponent(typeof(Cell));
            neighbours.Add(upperNeighbour);
        }
        
        if(le != null)
        {
            leftNeighbour = (Cell)le.GetComponent(typeof(Cell));
            neighbours.Add(leftNeighbour);
        }
        if(r != null)
        {
            rightNeighbour = (Cell)r.GetComponent(typeof(Cell));
            neighbours.Add(rightNeighbour);
        }
        if(lo != null)
        { 
            lowerNeighbour = (Cell)lo.GetComponent(typeof(Cell));
            neighbours.Add(lowerNeighbour);
        }
	}

	public void setOccupied(GameObject gObj)
	{
		objectOnCell = gObj;
		isOccupied = (gObj != null);
	}
}
