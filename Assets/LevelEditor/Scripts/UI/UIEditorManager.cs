using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIEditorManager : MonoBehaviour {

    Button unitB, objectB, brushB, leftRotateB, rightRotateB, removeB, spawnGovB, spawnRebB;
    Button newFileB, openFileB, saveFileB, undoB, redoB;
    GameObject assetBar, objectView, materialView;
    List<GameObject> objectButtons = new List<GameObject>();    //Liste aller Buttons, welche in der PropsLeiste angezeigt werden
    List<GameObject> objectPrefabs; //Liste der vorher als Prefabs festgelegten Props
    List<GameObject> materialButtons = new List<GameObject>();  //Liste aller Buttons, welche in der MaterialLeiste angezeigt werden
    List<Material> materials;   //Liste der vorher als Prefabs festgelegten Bodentexturen
    ObjectSetterHelperLE osh;
    Button pressedButton;

    public static Dictionary<string, GameObject> prefabs
        = new Dictionary<string, GameObject>();

    public static Dictionary<GameObject, string> prefabsInverse
        = new Dictionary<GameObject, string>();

    public static Dictionary<string, Material> groundMaterials
        = new Dictionary<string, Material>();

    public static Dictionary<Material, string> groundMaterialsInverse
        = new Dictionary<Material, string>();

    // Use this for initialization
    void Start () {
        osh = GameObject.Find("Plane").GetComponent<ObjectSetterHelperLE>();
       // unitB = GameObject.Find("UnitB").GetComponent<Button>();
       // unitB.onClick.AddListener(() => Methode());

        objectB = GameObject.Find("ObjectB").GetComponent<Button>();
        objectB.onClick.AddListener(() => {
            showAssetBar("objects");
            changeButtonColor(objectB);
            });

        brushB = GameObject.Find("BrushB").GetComponent<Button>();
        brushB.onClick.AddListener(() =>
        {
            showAssetBar("materials");
            changeButtonColor(brushB);
        });

        leftRotateB = GameObject.Find("RotateLeftB").GetComponent<Button>();
        leftRotateB.onClick.AddListener(() => osh.rotateLeft());

        rightRotateB = GameObject.Find("RotateRightB").GetComponent<Button>();
        rightRotateB.onClick.AddListener(() => osh.rotateRight());

        spawnGovB = GameObject.Find("SpawnGovB").GetComponent<Button>();
        spawnGovB.onClick.AddListener(() => {
            osh.activateGovSpawn();
            changeButtonColor(spawnGovB);
        });
       
        spawnRebB = GameObject.Find("SpawnRebB").GetComponent<Button>();
        spawnRebB.onClick.AddListener(() =>
        {
            osh.activateRebSpawn();
            changeButtonColor(spawnRebB);
        });

        removeB = GameObject.Find("RemoveB").GetComponent<Button>();
        removeB.onClick.AddListener(() =>
        {
            osh.activateDestroyTool();
            changeButtonColor(removeB);
        });

        saveFileB = GameObject.Find("SaveFileB").GetComponent<Button>();
        saveFileB.onClick.AddListener(() => {
                string path = DialogManager.SaveFileDialog();
                SavingScript save = FindObjectOfType<SavingScript>();
                if(save != null) {
                    save.saveLevel(path);
                }
            });


        openFileB = GameObject.Find("OpenFileB").GetComponent<Button>();
        openFileB.onClick.AddListener(() => {
        string path = DialogManager.OpenFileDialog();
        LoadingScript load = FindObjectOfType<LoadingScript>();
        if(load != null && !string.IsNullOrEmpty(path)) {
            LevelConfiguration peekedHeader = load.peekHeader(path);
            load.filePath = path;
            load.editorMode = true;
            if(peekedHeader.gridHeight == 20 && peekedHeader.gridWidth == 20) {
                    SceneManager.LoadScene("EmptyLevel 1");
                }
            else if(peekedHeader.gridHeight == 20 && peekedHeader.gridWidth == 40) {
                    SceneManager.LoadScene("EmptyLevel 2");
                }
            else if(peekedHeader.gridHeight == 40 && peekedHeader.gridWidth == 80) {
                    SceneManager.LoadScene("EmptyLevel 3");
                }
                LevelConfiguration.instance.gridHeight = peekedHeader.gridHeight;
                LevelConfiguration.instance.gridWidth = peekedHeader.gridWidth;
                LevelConfiguration.instance.defaultValues = false;
        }
    });
        
        newFileB = GameObject.Find("NewFileB").GetComponent<Button>();
        newFileB.onClick.AddListener(() => {
            LoadingScript load = FindObjectOfType<LoadingScript>();
            if(null != load) {
                load.filePath = null;
                load.editorMode = true;
                if (LevelConfiguration.instance.gridHeight == 20 && LevelConfiguration.instance.gridWidth == 20) {
                    SceneManager.LoadScene("EmptyLevel 1");
                }
                else if (LevelConfiguration.instance.gridHeight == 20 && LevelConfiguration.instance.gridWidth == 40) {
                    SceneManager.LoadScene("EmptyLevel 2");
                }
                else if (LevelConfiguration.instance.gridHeight == 40 && LevelConfiguration.instance.gridWidth == 80) {
                    SceneManager.LoadScene("EmptyLevel 3");
                }
            }
        });

        undoB = GameObject.Find("UndoB").GetComponent<Button>();
        undoB.onClick.AddListener(() => URManager.undo());

        redoB = GameObject.Find("RedoB").GetComponent<Button>();
        redoB.onClick.AddListener(() => URManager.redo());
        
        objectView = GameObject.Find("ObjectView");
        materialView = GameObject.Find("MaterialView");
        assetBar = GameObject.Find("Assetbar");

        //Initialisiere die ObjektLeiste
        initializeObjectView();

        //Initialisiere die MaterialLeiste
        initializeMaterialView();

        //Blende gesamte assetBar aus
        assetBar.SetActive(false);
    }

    void initializeMaterialView()
    {
        groundMaterials.Clear();
        groundMaterialsInverse.Clear();
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        foreach (Material mat in Resources.LoadAll<Material>(Constants.GROUND_MATERIAL_PATH))
        {
            string name = mat.name;
            groundMaterialsInverse.Add(mat, name);
        }
        //Speichere alle Materials in die Liste
        materials = new List<Material>(groundMaterialsInverse.Keys);

        int counter = 0;
        //X-Startposition
        float startX = -636;

        foreach (Material key in materials)
        {
            
            //Erstelle neues GameObject für Button
            GameObject temp = new GameObject();
            //Füge ImageComponent hinzu
            temp.AddComponent<Image>();
            //Lese Bild aus Preview aus
            Texture2D texture = null;

            //Liest die Textur aus der AssetPreview im Editor aus und setzt diese
            while (texture == null)
                texture = AssetPreview.GetAssetPreview(key);


            //Konvertiere Textur zu Sprite
            Sprite textureSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //Füge Textur hinzu
            temp.GetComponent<Image>().sprite = textureSprite;
            //Passe Größe an
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            temp.transform.SetParent(GameObject.Find("MaterialBar").GetComponent<Transform>());

            //Füge Button seiner zugehörigen ButtonListe hinzu
            materialButtons.Add(temp);
            temp.AddComponent<Button>().onClick.AddListener(() =>
            {
                osh.activateBrushTool(materials[materialButtons.IndexOf(temp)]);
            });
            if (counter == 0)
                temp.transform.localPosition = new Vector2(startX, 0);
            else
                temp.transform.localPosition = new Vector2(startX + counter * 10, 0);
            temp.name = "Button " + counter;
            startX += 165;
            counter++;
        }
    }

    //Verhält sich wie initializeMaterialsView
    void initializeObjectView()
    {
        prefabs.Clear();
        prefabsInverse.Clear();
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        foreach (GameObject prefab in Resources.LoadAll<GameObject>(Constants.PROPS_PREFAB_PATH))
        {
            string prefabName = prefab.name;
            bool correctTag = false;
            for (int i = 0; i < 2; ++i)
            {
                string str = Constants.FILE_LEVEL_ITEM_TAGS[i];
                if (prefab.tag.Equals(str))
                {
                    correctTag = true;
                    break;
                }
            }
            if (!correctTag)
            {
                continue;
            }
            prefabs.Add(prefabName, prefab);
            prefabsInverse.Add(prefab, prefabName);
        }

        objectPrefabs = new List<GameObject>(prefabsInverse.Keys);


        int counter = 0;
        float startX = -2100;
        
        RectTransform objectRect = GameObject.Find("ObjectBar").GetComponent<RectTransform>();
        objectRect.sizeDelta = new Vector2(objectPrefabs.Count * 250, 0);


        foreach (GameObject key in objectPrefabs)
        {
            //Erstelle neues GameObject für Button
            GameObject temp = new GameObject();
            //Füge ImageComponent hinzu
            temp.AddComponent<Image>();
            //Lese Bild aus Preview aus
            Texture2D texture = null;
            

            while (texture == null)
                texture = AssetPreview.GetAssetPreview(key);


            //Konvertiere Textur zu Sprite
            Sprite textureSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //Füge Textur hinzu
            temp.GetComponent<Image>().sprite = textureSprite;
            //Passe Größe an
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            GameObject parent = GameObject.Find("ObjectBar");
            temp.transform.SetParent(parent.transform, false);
            RectTransform tempRect = temp.GetComponent<RectTransform>();
            //tempRect.anchorMin = new Vector2(0, 1);
            //tempRect.anchorMax = new Vector2(0, 1);
            objectButtons.Add(temp);
            temp.AddComponent<Button>().onClick.AddListener(() =>
            {
                osh.activatePlacingTool(objectPrefabs[objectButtons.IndexOf(temp)]);
            });
            if (counter == 0)
            {
                tempRect.localPosition = new Vector2(startX,0);
            }
           tempRect.localPosition = new Vector2(startX + counter * 10, 0);
            temp.name = "Button " + counter;
            startX += 165;
            counter++;

          
            
        }
    }

    //Wird aufgerufen um die unterste Leiste ein- und auszublenden
    void showAssetBar(string viewName)
    {
        if(null != assetBar)
            if (!assetBar.activeSelf)
                assetBar.SetActive(true);

        switch (viewName)
        {
            case "objects":
                objectView.SetActive(true);
                materialView.SetActive(false);
                break;

            case "materials":
                objectView.SetActive(false);
                materialView.SetActive(true);
                break;

            default:
                break;
        }
        Scrollbar scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();

        //Schiebe den Value der Scrollbar nach links (0.0f)
        scrollbar.value = 0.0f;
    }

    //Ändert die Farbe eines Buttons, falls er angewählt wurde und setzt die Farbe auf seine ursprünglichen Werte
    //falls er wieder abgewählt wurde
    void changeButtonColor(Button btn)
    {
        if (btn != pressedButton)
        {
            btn.GetComponent<Image>().color = Color.yellow;
            if(pressedButton != null)
                pressedButton.GetComponent<Image>().color = new Color(225, 225, 225, 255);
            pressedButton = btn;
        }
        else
        {
            btn.GetComponent<Image>().color = new Color(225, 225, 225, 255);
            pressedButton = null;
            assetBar.SetActive(false);
        }

        
    }
    

}
