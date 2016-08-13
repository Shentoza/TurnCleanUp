using UnityEngine;
using System.Collections;

public class ObjectSetterHelperLE : MonoBehaviour {

    Collider highlightedCell;
    [SerializeField]
    Material highlightedMat;
    [SerializeField]
    Material notSelected;
    [SerializeField]
    LayerMask Cellmask;

    [SerializeField]
    GameObject testObjekt;
    GameObject test;

    [SerializeField]
    ObjectSetterLE obsLE;
    [SerializeField]
    BattlefieldCreatorLE bfcLE;
    [SerializeField]
    CameraRotationScript crs;

    //Komponenten des zu platzierenden Objekts
    Collider testCOL;
    MeshRenderer testmr;
    ObjectComponent testOC;
    Transform testTrans;
    Material originalMat;

    //Zellen des Battlefields
    GameObject[,] Zellen;

    bool killObject;
    bool placeMode;
    bool canPlace;

    // Use this for initialization
    void Start () {
        Zellen = bfcLE.getZellen();
    }
	
	// Update is called once per frame
	void Update () {

        mouseHover();

        //Objekt im PlaceMode platzieren
        if (Input.GetMouseButtonDown(0) && highlightedCell && placeMode && canPlace)
        {
            testCOL.enabled = true;
            GameObject newObject = Instantiate(test);
            newObject.GetComponent<MeshRenderer>().material = originalMat;
            testCOL.enabled = false;
            obsLE.moveObject(bfcLE.getZellen(), highlightedCell.GetComponent<Cell>().xCoord,
                highlightedCell.GetComponent<Cell>().zCoord, newObject, true);

            if(!Input.GetKey("left shift"))
            {
                placeMode = false;
                destroyTestObject();
            }
        }

        //Platziertes Objekt Zuerstören
        if(Input.GetKeyDown("k"))
        {
            killObject = true;
        }
        if(Input.GetKeyDown("p"))
        {
            if (placeMode)
            {
                placeMode = false;
                destroyTestObject();
            }
            else
            {
                placeMode = true;
                setNewTest(testObjekt);
                placingHelper();
            }
        }

        //rotieren von Objekten
        if(placeMode && Input.GetKeyDown("u"))
        {
            rotateTest();
        }


        //Für die Kamera
        if(Input.GetMouseButtonDown(2))
        {
            crs.setStartRotation();
        }
        if(Input.GetMouseButtonUp(2))
        {
            crs.setStopRotation();
        }

    }

    void mouseHover()
    {
        Ray mouseOver = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hover;
        Physics.Raycast(mouseOver, out hover, Mathf.Infinity);

        if (hover.collider != null)
        {
            //Cursor über neuer Zelle
            if (hover.collider.gameObject.tag == "Cell")
            {
                if (hover.collider != highlightedCell)
                {
                    if (highlightedCell != null)
                    {
                        MeshRenderer mrh = highlightedCell.GetComponent<MeshRenderer>();
                        mrh.material = notSelected;
                    }
                    MeshRenderer mr = hover.collider.gameObject.GetComponent<MeshRenderer>();
                    mr.material = highlightedMat;
                    highlightedCell = hover.collider;
                    if(placeMode)
                    {
                        placingHelper();
                    }
                }
                if(hover.collider == highlightedCell)
                {
                    MeshRenderer mr = hover.collider.gameObject.GetComponent<MeshRenderer>();
                    mr.material = highlightedMat;
                    highlightedCell = hover.collider;
                }
            }
            //Cursor über selber Zelle
            else if(hover.collider.gameObject.tag != "Cell")
            {
                if (highlightedCell != null)
                {
                    MeshRenderer mr = highlightedCell.GetComponent<MeshRenderer>();
                    mr.material = notSelected;
                    highlightedCell = null;
                    //Löscht Objekt
                    if (killObject)
                    {
                        destroyObject(hover);
                    }
                }
            }
        }
        //Curser über keiner Zelle
        else
        {
            if (highlightedCell != null)
            {
                MeshRenderer mr = highlightedCell.GetComponent<MeshRenderer>();
                mr.material = notSelected;
                highlightedCell = null;
            }
            //destroyTestObject();
        }
    }

    //Zerstört das Testobjekt im placeMode
    void destroyTestObject()
    {
        Destroy(test);
        testCOL = null;
        testmr = null;
        testOC = null;
        originalMat = null;
    }

    //Hilft beim platzieren von Objekten("Geistobjekt")
    void placingHelper()
    {
        int x = highlightedCell.GetComponent<Cell>().xCoord;
        int z = highlightedCell.GetComponent<Cell>().zCoord;
        canPlace = true;
        bool moveable = true;

        testCOL.enabled = false;

        if (x > (bfcLE.sizeX * 10) - (testOC.sizeX) || z > (bfcLE.sizeZ * 10) - (testOC.sizeZ))
        {
            canPlace = false;
            moveable = false;
        }
        if ((bfcLE.sizeZ * 10) > (z + testOC.sizeZ - 1) && (bfcLE.sizeX * 10) > (x + testOC.sizeX - 1) && canPlace)
        {
            for (int i = x; i < x + testOC.sizeX; i++)
            {
                for (int j = z; j < z + testOC.sizeZ; j++)
                {
                    if (Zellen[i,j].GetComponent<Cell>().isOccupied == true)
                    {
                        canPlace = false;
                    }
                }
            }
        }
        if (canPlace && moveable)
        {
            testmr.material = highlightedMat;

            Vector3 posi = Zellen[x + testOC.sizeX - 1, z + testOC.sizeZ - 1].transform.position - Zellen[x, z].transform.position;
            posi /= 2;
            posi += Zellen[x, z].transform.position;

            testTrans.position = new Vector3(posi.x, testTrans.position.y, posi.z);
        }
        else if(moveable && !canPlace)
        {
            testmr.material = notSelected;

            Vector3 posi = Zellen[x + testOC.sizeX - 1, z + testOC.sizeZ - 1].transform.position - Zellen[x, z].transform.position;
            posi /= 2;
            posi += Zellen[x, z].transform.position;

            testTrans.position = new Vector3(posi.x, testTrans.position.y, posi.z);
        }
        if(!moveable)
        {
            testmr.material = notSelected;
        }

    }

    //Objekt rotieren Methode
    void rotateTest()
    {
        int oldX;
        int oldZ;

        testTrans.Rotate(new Vector3(testTrans.rotation.x, testTrans.rotation.y + 90, testTrans.rotation.z));
        oldX = testOC.sizeX;
        oldZ = testOC.sizeZ;

        testOC.sizeZ = oldX;
        testOC.sizeX = oldZ;

        placingHelper();
    }

    //Löschen Methode
    void destroyObject(RaycastHit toDestroy)
    {
        ObjectComponent killOC = toDestroy.collider.gameObject.GetComponent<ObjectComponent>();
        int x = killOC.cell.xCoord;
        int z = killOC.cell.zCoord;
        GameObject[,] Zellen = bfcLE.getZellen();
        canPlace = true;


        for (int i = x; i < x + killOC.sizeX; i++)
        {
            for (int j = z; j < z + killOC.sizeZ; j++)
            {
                Zellen[i, j].GetComponent<Cell>().isOccupied = false;
                Zellen[i, j].GetComponent<Cell>().hoheDeckung = false;
                Zellen[i, j].GetComponent<Cell>().niedrigeDeckung = false;
            }
        }

        Destroy(toDestroy.collider.gameObject);
        killObject = false;
    }

    void setNewTest(GameObject newObject)
    {
        test = Instantiate(newObject);
        testCOL = test.GetComponent<Collider>();
        testmr = test.GetComponent<MeshRenderer>();
        testOC = test.GetComponent<ObjectComponent>();
        testTrans = test.GetComponent<Transform>();
        originalMat = Instantiate(testmr.material);
    }
}
