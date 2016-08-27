using UnityEngine;
using System.Collections;

public class ObjectSetterHelperLE : MonoBehaviour {

    Collider highlightedCell;
    [SerializeField]
    Material highlightedMat;
    [SerializeField]
    Material notSelected;
    [SerializeField]
    Material cantPlaceMat;
    [SerializeField]
    Material brushMaterial;

    [SerializeField]
    LayerMask Cellmask;
    [SerializeField]
    LayerMask Farbmask;

    GameObject test;

    [SerializeField]
    GameObject RebPlaceholder;
    [SerializeField]
    GameObject GovPlaceholder;

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
    bool brushMode;

    bool placeRebSpwn;
    int countReb = 0;
    bool placeGovSpwn;
    int countGov = 0;

    // Use this for initialization
    void Start () {
        Zellen = bfcLE.getZellen();
    }
	
	// Update is called once per frame
	void Update () {

        bool isGUIelement = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);

        mouseHover();

        //Objekt im PlaceMode platzieren
        if (Input.GetMouseButtonDown(0) && highlightedCell && placeMode && canPlace && !isGUIelement)
        {
            testCOL.enabled = true;
            GameObject newObject = Instantiate(test);
            newObject.GetComponent<MeshRenderer>().material = originalMat;
            testCOL.enabled = false;
            obsLE.moveObject(bfcLE.getZellen(), highlightedCell.GetComponent<Cell>().xCoord,
                highlightedCell.GetComponent<Cell>().zCoord, newObject, true);
            newObject.GetComponent<ObjectSetter>().x = highlightedCell.GetComponent<Cell>().xCoord;
            newObject.GetComponent<ObjectSetter>().z = highlightedCell.GetComponent<Cell>().zCoord;

            newObject.GetComponent<ObjectComponent>().original = test;

            if(!Input.GetKey("left shift"))
            {
                placeMode = false;
                destroyTestObject();
            }
        }
        else if(Input.GetMouseButtonDown(0) && highlightedCell && canPlace && placeGovSpwn || placeRebSpwn && !isGUIelement)
        {
            testCOL.enabled = true;
            GameObject newObject = Instantiate(test);
            newObject.GetComponent<MeshRenderer>().material = originalMat;
            testCOL.enabled = false;
            obsLE.moveObject(bfcLE.getZellen(), highlightedCell.GetComponent<Cell>().xCoord,
                highlightedCell.GetComponent<Cell>().zCoord, newObject, true);

            if (placeRebSpwn)
            {
                bfcLE.startPostionsP1.Add(new Vector2(highlightedCell.GetComponent<Cell>().xCoord, highlightedCell.GetComponent<Cell>().zCoord));
            }
            if (placeGovSpwn)
            {
                bfcLE.startPostionsP2.Add(new Vector2(highlightedCell.GetComponent<Cell>().xCoord, highlightedCell.GetComponent<Cell>().zCoord));
            }
        }
        else if((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && highlightedCell && brushMode && !isGUIelement)
        {
            brushTool();
        }


        //Für die Kamera
        if(Input.GetMouseButtonDown(2) && !isGUIelement)
        {
            crs.setStartRotation();
        }
        if(Input.GetMouseButtonUp(2))
        {
            crs.setStopRotation();
        }

        //Deaktiviert alle Modi
        if(Input.GetMouseButtonDown(1))
        {
            if (placeMode)
            {
                activatePlacingTool(null);
            }
            if (killObject)
            {
                activateDestroyTool();
            }
            if (placeGovSpwn)
            {
                activateGovSpawn();
            }
            if (placeRebSpwn)
            {
                activateRebSpawn();
            }
            if(brushMode)
            {
                activateBrushTool(null);
            }
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
                    if(placeMode || placeGovSpwn || placeRebSpwn)
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
        if (highlightedCell != null)
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
                        if (Zellen[i, j].GetComponent<Cell>().isOccupied == true)
                        {
                            canPlace = false;
                        }
                    }
                }
            }
            if (canPlace && moveable)
            {
                for (int i = 0; i < testmr.materials.Length; i++)
                {
                    testmr.materials[i] = highlightedMat;
                }

                Vector3 posi = Zellen[x + testOC.sizeX - 1, z + testOC.sizeZ - 1].transform.position - Zellen[x, z].transform.position;
                posi /= 2;
                posi += Zellen[x, z].transform.position;

                testTrans.position = new Vector3(posi.x, testTrans.position.y, posi.z);
            }
            else if (moveable && !canPlace)
            {
                for (int i = 0; i < testmr.materials.Length; i++)
                {
                    testmr.materials[i] = cantPlaceMat;
                }

                Vector3 posi = Zellen[x + testOC.sizeX - 1, z + testOC.sizeZ - 1].transform.position - Zellen[x, z].transform.position;
                posi /= 2;
                posi += Zellen[x, z].transform.position;

                testTrans.position = new Vector3(posi.x, testTrans.position.y, posi.z);
            }
            if (!moveable)
            {
                for (int i = 0; i < testmr.materials.Length; i++)
                {
                    testmr.materials[i] = cantPlaceMat;
                }
            }
        }

    }

    //Löschen Methode
    void destroyObject(RaycastHit toDestroy)
    {
        GameObject kollel = toDestroy.collider.gameObject;

        destroyObject(kollel);
    }


    //Setzt ein neues Objekt als Platzierungobjekt
    void setNewTest(GameObject newObject)
    {
        test = Instantiate(newObject);
        testCOL = test.GetComponent<Collider>();
        if(!test.GetComponent<MeshRenderer>())
        {
            testmr = test.GetComponentInChildren<MeshRenderer>();
        }
        else
        {
            testmr = test.GetComponent<MeshRenderer>();
        }
        testOC = test.GetComponent<ObjectComponent>();
        testTrans = test.GetComponent<Transform>();
        originalMat = Instantiate(testmr.material);
    }

    //Führt das Färben von Zellen aus
    void brushTool()
    {
        if (!placeMode && !killObject && !placeGovSpwn && !placeRebSpwn)
        {
            Ray mouseOver = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit farbSelect;
            Physics.Raycast(mouseOver, out farbSelect, Mathf.Infinity, Farbmask);

            if (farbSelect.collider != null)
            {
                MeshRenderer farbMR = farbSelect.collider.gameObject.GetComponent<MeshRenderer>();

                farbMR.material = brushMaterial;
            }
        }
    }


    //Alle Methoden zum Aktiviren von Modi
    //Werden über UI aufgerufen

    //Aktiviert BrushTool
    public void activateBrushTool(Material newMat)
    {
        if (placeMode)
        {
            activatePlacingTool(null);
        }
        if (killObject)
        {
            activateDestroyTool();
        }
        if (placeGovSpwn)
        {
            activateGovSpawn();
        }
        if (placeRebSpwn)
        {
            activateRebSpawn();
        }

        if (!placeMode && !killObject && !placeGovSpwn && !placeRebSpwn)
        { 
            if (brushMode && brushMaterial == newMat || brushMaterial == null)
            {
               brushMode = false;
            }
            else
            {
                brushMode = true;
                brushMaterial = newMat;
            }
        }
    }

    //Aktiviert DestroyTool
    public void activateDestroyTool()
    {
        if (placeMode)
        {
            activatePlacingTool(null);
        }
        if (brushMode)
        {
            activateBrushTool(null);
        }
        if (placeGovSpwn)
        {
            activateGovSpawn();
        }
        if (placeRebSpwn)
        {
            activateRebSpawn();
        }

        if (!placeMode && !brushMode && !placeGovSpwn && !placeRebSpwn)
        {
            if(killObject)
            {
                killObject = false;
            }
            else
            {
                killObject = true;
            }
        }
    }

    //Aktiviert Platzierungsmodus
    public void activatePlacingTool(GameObject newTestObjekt)
    {
        Debug.Log("Hallo");
        if (brushMode)
        {
            activateBrushTool(null);
        }
        if (killObject)
        {
            activateDestroyTool();
        }
        if (placeGovSpwn)
        {
            activateGovSpawn();
        }
        if (placeRebSpwn)
        {
            activateRebSpawn();
        }
        if (!killObject && !brushMode && !placeGovSpwn && !placeRebSpwn)
        {
            if (placeMode && newTestObjekt == null || newTestObjekt == test)
            {
                placeMode = false;
                destroyTestObject();
            }
            else
            {
                placeMode = true;
                setNewTest(newTestObjekt);
                placingHelper();
            }
        }
    }

    //Methode zum Links rotieren
    public void rotateLeft()
    {
        if (placeMode && !killObject && !brushMode && !placeGovSpwn && !placeRebSpwn)
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
    }

    //Methode zum Rechts rotieren
    public void rotateRight()
    {
        if (placeMode && !killObject && !brushMode && !placeGovSpwn && !placeRebSpwn)
        {
            int oldX;
            int oldZ;

            testTrans.Rotate(new Vector3(testTrans.rotation.x, testTrans.rotation.y - 90, testTrans.rotation.z));
            oldX = testOC.sizeX;
            oldZ = testOC.sizeZ;

            testOC.sizeZ = oldX;
            testOC.sizeX = oldZ;

            placingHelper();
        }
    }

    //Methode um Spawnplätze für Regierung zu Platzieren
    public void activateGovSpawn()
    {
        if (placeMode)
        {
            activatePlacingTool(null);
        }
        if (killObject)
        {
            activateDestroyTool();
        }
        if (brushMode)
        {
            activateBrushTool(null);
        }
        if (placeRebSpwn)
        {
            activateRebSpawn();
        }

        if (!killObject && !placeMode && !brushMode && !placeRebSpwn)
        {
            if (!placeGovSpwn)
            {
                placeGovSpwn = true;
                setNewTest(GovPlaceholder);
                placingHelper();
            }
            else
            {
                placeGovSpwn = false;
                destroyTestObject();
            }
        }
    }

    //Methode um Spawnplätze für Rebellen zu Platzieren
    public void activateRebSpawn()
    {
        if (placeMode)
        {
            activatePlacingTool(null);
        }
        if (killObject)
        {
            activateDestroyTool();
        }
        if (placeGovSpwn)
        {
            activateGovSpawn();
        }
        if (brushMode)
        {
            activateBrushTool(null);
        }

        if (!killObject && !placeMode && !brushMode && !placeGovSpwn)
        {
            if (!placeRebSpwn)
            {
                placeRebSpwn = true;
                setNewTest(RebPlaceholder);
                placingHelper();
            }
            else
            {
                placeRebSpwn = false;
                destroyTestObject();
            }
        }
    }


    //Überladene Methoden für UndoRedo
    void destroyObject(GameObject toDestroy)
    {
        ObjectComponent killOC = toDestroy.GetComponent<ObjectComponent>();
        int x = killOC.cell.xCoord;
        int z = killOC.cell.zCoord;
        GameObject[,] Zellen = bfcLE.getZellen();

        if (toDestroy.tag == "RebPlaceholder")
        {
            Vector2 coords = new Vector2(x, z);

            bfcLE.startPostionsP1.Remove(coords);

            countReb--;
        }
        if (toDestroy.tag == "GovPlacerholder")
        {
            Vector2 coords = new Vector2(x, z);

            bfcLE.startPostionsP2.Remove(coords);

            countGov--;
        }
        for (int i = x; i < x + killOC.sizeX; i++)
        {
            for (int j = z; j < z + killOC.sizeZ; j++)
            {
                Zellen[i, j].GetComponent<Cell>().isOccupied = false;
                Zellen[i, j].GetComponent<Cell>().hoheDeckung = false;
                Zellen[i, j].GetComponent<Cell>().niedrigeDeckung = false;
            }
        }
    }


}
