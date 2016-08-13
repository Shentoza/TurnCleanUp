using UnityEngine;
using System.Collections;

public class ObjectSetterLE : MonoBehaviour {

	Transform objecttrans;
	GameObject zelle;
	Transform zelletrans;
	Cell cell;

	float posiX;
	float posiZ;
    //float gridHeight;

	// Use this for initialization
	void Start () {
        //BattlefieldCreater battlefield = (BattlefieldCreater)GameObject.FindObjectOfType(typeof(BattlefieldCreater));
        //gridHeight = battlefield.gridHeight;
	}
	
	// Update is called once per frame
	void Update () {

    }	


	//Setzt das Spielerobjekt an dem das Script hängt auf die Zelle
	public void move(GameObject[,] Zellen, int x, int z)
	{
		//Objekt Transform und AttributeComponent
		objecttrans = (Transform)this.gameObject.GetComponent (typeof(Transform));
		AttributeComponent objectAttr = (AttributeComponent)this.gameObject.GetComponent (typeof(AttributeComponent));

		zelle = Zellen[x, z];
		zelletrans = (Transform) zelle.GetComponent (typeof(Transform));
		Cell zellecell = (Cell)zelle.GetComponent (typeof(Cell));
		zellecell.setOccupied (this.gameObject);
		objecttrans.position = new Vector3 (zelletrans.position.x, objecttrans.position.y, zelletrans.position.z);
		objectAttr.setCurrentCell (zellecell);
	}


	//Setzt Objekte auf richtige Zelle, setzt deckung etc
	public void moveObject(GameObject[,] Zellen, int x, int z, GameObject p_Object, bool placable)
	{
		Transform objectTrans = (Transform)p_Object.GetComponent (typeof(Transform));
		ObjectComponent objectComp = (ObjectComponent)p_Object.GetComponent (typeof(ObjectComponent));

        //Setzt alle Zellen auf occupied, deckung etc sofern das Objekt mehr als eine Zelle in x oder z einnimmt.
        // zB an Stelle 1, 1 und groesse 2*2 waere dann:
        // 0 0 0 0
        // 0 x x 0
        // 0 x x 0
        // 0 0 0 0

        Vector3 posi = Zellen[x + objectComp.sizeX - 1, z + objectComp.sizeZ -1].transform.position - Zellen[x, z].transform.position;
        posi /= 2;
        posi += Zellen[x, z].transform.position;

        objectComp.cell = Zellen[x, z].GetComponent<Cell>();

        objectTrans.position = new Vector3(posi.x, objectTrans.position.y, posi.z) ;


        if ((objectComp.sizeX > 1 || objectComp.sizeZ > 1) && placable) {
			for (int i = 0; i < objectComp.sizeX; i++) {
				zelle = Zellen [x + i, z];
				cell = (Cell)zelle.GetComponent (typeof(Cell));

				if (objectComp.isOccupant) {
					cell.setOccupied (this.gameObject);
				}
				if (objectComp.niedrigeDeckung) {
					cell.niedrigeDeckung = true;
				}
				if (objectComp.hoheDeckung) {
					cell.hoheDeckung = true;
				}
				if (objectComp.sizeZ > 1) {
					for (int j = 1; j < objectComp.sizeZ; j++) {
						zelle = Zellen [x + i, z + j];
						cell = (Cell)zelle.GetComponent (typeof(Cell));

						if (objectComp.isOccupant) {
							cell.setOccupied (this.gameObject);
						}
						if (objectComp.niedrigeDeckung) {
							cell.niedrigeDeckung = true;
						}
						if (objectComp.hoheDeckung) {
							cell.hoheDeckung = true;
						}
					}
				}
			}
		}
		//Falls die groesse des Objekts 1*1 ist
		else if(placable)
		{
			zelle = Zellen [x, z];
			cell = (Cell)zelle.GetComponent (typeof(Cell));

			if (objectComp.isOccupant) {
				cell.setOccupied (this.gameObject);
			}
			if (objectComp.niedrigeDeckung) {
				cell.niedrigeDeckung = true;
			}
			if (objectComp.hoheDeckung) {
				cell.hoheDeckung = true;
			}
		}
	}
}
