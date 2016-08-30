using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlefieldCreatorLE : MonoBehaviour
{

    public static BattlefieldCreatorLE instance { get; private set; }

    Transform transformPlane;
    public float sizeX;
    public float sizeZ;
    GameObject[,] Zellen;
    public GameObject[,] Farbzellen { get; private set; }
    public Material material;
    public Material farbMat;
    public Material woodCubeMaterial;
    public float gridHeight;

    static public int mapSizeX;
    static public int mapSizeZ;

    public List<Vector2> startPostionsP1;
    public List<Vector2> startPostionsP2;

    public static bool Initialize()
    {
        if (instance != null)
            Destroy(instance);
        GameObject bfc = new GameObject();
        instance = bfc.AddComponent<BattlefieldCreatorLE>();
        instance.transformPlane = instance.GetComponent<Transform>();
        instance.initiateBattlefield();

        return true;
    }

    void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        instance.transformPlane = instance.GetComponent<Transform>();
        instance.initiateBattlefield();
    }

    void initiateBattlefield()
    {
        //Bitte kein 1.4 oder ein vielfaches davon benutzen. Danke!
        sizeX = transformPlane.localScale.x;
        sizeZ = transformPlane.localScale.z;

        //Prüft ob die größe der Plane größer 1 ist
        if (sizeX < 1) {
            transformPlane.localScale = new Vector3(1, 0, transformPlane.localScale.z);
        }
        if (sizeZ < 1) {
            transformPlane.localScale = new Vector3(transformPlane.localScale.x, 0, 1);
        }

        //Runden die größe der Plane auf erste nachkommastelle
        sizeX = Mathf.Round(sizeX * 10);
        sizeX = sizeX / 10;

        sizeZ = Mathf.Round(sizeZ * 10);
        sizeZ = sizeZ / 10;

        int sizeZint = (int)(sizeZ * 10);
        int sizeXint = (int)(sizeX * 10);

        mapSizeX = sizeXint;
        mapSizeZ = sizeZint;

        transformPlane.localScale = new Vector3(sizeX, 1, sizeZ);

        Zellen = new GameObject[(int)(sizeXint), (int)(sizeZint)];
        Farbzellen = new GameObject[(int)(sizeXint), (int)(sizeZint)];

        //Verschiebt Plane in den 0 Punkt(Oben links)
        transformPlane.position = new Vector3(sizeX * 5, 0, sizeZ * -5);

        LayerMask cellLayer = LayerMask.NameToLayer("Cell");
        LayerMask farbLayer = LayerMask.NameToLayer("Farbzelle");

        GameObject brett = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brett.transform.position = new Vector3(transformPlane.position.x, transformPlane.position.y - 1.05f, transformPlane.position.z);
        brett.transform.localScale = new Vector3((float)sizeX * 10, 2, (float)sizeZ * 10);
        brett.GetComponent<MeshRenderer>().material = woodCubeMaterial;


        //Initialisiert alle Zellen
        for (float z = 0; z > -(sizeZint); z--) {
            for (float x = 0; x < (sizeXint); x++) {
                GameObject zelle = GameObject.CreatePrimitive(PrimitiveType.Quad);
                zelle.transform.Rotate(new Vector3(90, 0, 0));
                zelle.AddComponent<Cell>();
                zelle.tag = "Cell";
                zelle.name = x + "|" + -z;
                zelle.transform.position = new Vector3((x + 0.5f), 0.002f, (z - 0.5f));
                MeshRenderer mr = (MeshRenderer)zelle.GetComponent(typeof(MeshRenderer));
                mr.receiveShadows = false;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                mr.material = material;
                mr.enabled = true;

                BoxCollider box = (BoxCollider)zelle.AddComponent(typeof(BoxCollider));
                box.size = new Vector3(1, 1, .25f);
                box.center = new Vector3(0, 0, box.size.z / 2.0f);
                box.isTrigger = true;

                Cell cell = (Cell)zelle.GetComponent<Cell>();
                cell.xCoord = (int)x;
                cell.zCoord = (int)-z;

                zelle.layer = cellLayer;

                Zellen[(int)x, (int)-z] = zelle;

                //Zellen für farbveränderung
                GameObject farbzelle = GameObject.CreatePrimitive(PrimitiveType.Quad);
                farbzelle.transform.Rotate(new Vector3(90, 0, 0));
                farbzelle.layer = farbLayer;
                farbzelle.name = "Farbzelle: " + x + "|" + -z;
                farbzelle.transform.position = new Vector3((x + 0.5f), 0.001f, (z - 0.5f));
                MeshRenderer farbmr = (MeshRenderer)farbzelle.GetComponent(typeof(MeshRenderer));
                farbmr.receiveShadows = true;
                farbmr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                farbmr.material = farbMat;
                farbmr.enabled = true;

                BoxCollider farbbox = (BoxCollider)farbzelle.AddComponent(typeof(BoxCollider));
                farbbox.size = new Vector3(1, 1, .25f);
                farbbox.center = new Vector3(0, 0, farbbox.size.z / 2.0f);
                farbbox.isTrigger = true;

                Farbzellen[(int)x, (int)-z] = farbzelle;
            }
        }

        //Setzt die Nachbarn der Zellen
        for (int i = 0; i < (sizeZint); i++) {
            for (int j = 0; j < (sizeXint); j++) {
                Cell currentCell = (Cell)Zellen[j, i].GetComponent(typeof(Cell));
                GameObject upper = i - 1 >= 0 ? Zellen[j, (i - 1)] : null;
                GameObject lower = i + 1 < (sizeZint) ? Zellen[j, (i + 1)] : null;
                GameObject left = j - 1 >= 0 ? Zellen[(j - 1), i] : null;
                GameObject right = j + 1 < (sizeXint) ? Zellen[(j + 1), i] : null;

                currentCell.setNeighbours(upper, left, right, lower);
            }
        }

        /*
		//Setzt Objekte an richtiche Stelle
		ObjectSetter[] os = FindObjectsOfType (typeof(ObjectSetter)) as ObjectSetter[];
		foreach (ObjectSetter obs in os) 
		{
			obs.move (Zellen);
		}*/

        /*
		AttributeComponent[] attribComp = FindObjectsOfType (typeof(AttributeComponent)) as AttributeComponent[];
		foreach (AttributeComponent abc in attribComp) 
		{
			ObjectSetterLE obs = (ObjectSetterLE) abc.gameObject.GetComponent (typeof(ObjectSetterLE));
            //obs.move (Zellen);
            Debug.Log(obs);
		}

		ObjectComponent[] objectComp = FindObjectsOfType (typeof(ObjectComponent)) as ObjectComponent[];
		foreach (ObjectComponent obc in objectComp) 
		{
			ObjectSetterLE obs = (ObjectSetterLE) obc.gameObject.GetComponent (typeof(ObjectSetterLE));
			//obs.moveObject (Zellen);
		}*/
    }

    public Cell getCell(int x, int y)
    {
        return (Cell)Zellen[x, y].GetComponent(typeof(Cell));
    }

    public GameObject[,] getZellen()
    {
        return Zellen;
    }
}
