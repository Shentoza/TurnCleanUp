using UnityEngine;
using System.Collections;

//Dieses Script dient zur beschreibung der Objekte
//Hier wird die groesse der Objekte in X und Z Achse bestimmt, ob es Deckung bietet und eine Zelle belegt

public class ObjectComponent : MonoBehaviour {

	public Cell cell;
	public bool keineDeckung;
	public bool niedrigeDeckung;
	public bool hoheDeckung;

	public bool isOccupant;

	public int sizeX;
	public int sizeZ;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
