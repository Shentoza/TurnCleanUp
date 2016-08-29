using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIEditorManager : MonoBehaviour {

    Button unitB, objectB, brushB, leftRotateB, rightRotateB, removeB, spawnGovB, spawnRebB;
    Button newFileB, openFileB, saveFileB;
    GameObject assetBar, objectView, materialView;
    List<GameObject> objectButtons = new List<GameObject>();
    List<GameObject> objectPrefabs;
    List<GameObject> materialButtons = new List<GameObject>();
    List<Material> materials;
    ObjectSetterHelperLE osh;

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
        objectB.onClick.AddListener(() => showAssetBar("objects"));

        brushB = GameObject.Find("BrushB").GetComponent<Button>();
        brushB.onClick.AddListener(() => showAssetBar("materials"));

        leftRotateB = GameObject.Find("RotateLeftB").GetComponent<Button>();
        leftRotateB.onClick.AddListener(() => osh.rotateLeft());

        rightRotateB = GameObject.Find("RotateRightB").GetComponent<Button>();
        rightRotateB.onClick.AddListener(() => osh.rotateRight());

        spawnGovB = GameObject.Find("SpawnGovB").GetComponent<Button>();
        spawnGovB.onClick.AddListener(() => osh.activateGovSpawn());

        spawnRebB = GameObject.Find("SpawnRebB").GetComponent<Button>();
        spawnRebB.onClick.AddListener(() => osh.activateRebSpawn());



        removeB = GameObject.Find("RemoveB").GetComponent<Button>();
        removeB.onClick.AddListener(() => osh.activateDestroyTool());

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
            if(load != null) {
                load.filePath = path;
                load.editorMode = true;
                SceneManager.LoadScene("Empty Level");
            }
        });

        newFileB = GameObject.Find("NewFileB").GetComponent<Button>();
        newFileB.onClick.AddListener(() => {
            LoadingScript load = FindObjectOfType<LoadingScript>();
            if(null != load) {
                load.filePath = null;
                load.editorMode = true;
                SceneManager.LoadScene("Empty Level");
            }
        });


        objectView = GameObject.Find("ObjectView");
        materialView = GameObject.Find("MaterialView");
        assetBar = GameObject.Find("Assetbar");

        

        initializeObjectView();
        initializeMaterialView();
        assetBar.SetActive(false);

        /*objectBar = GameObject.Find("ObjectBar");

        for (int i = 0; i < objectBar.transform.childCount; i++)
        {
            GameObject temp = objectBar.transform.GetChild(i).gameObject;
            temp.GetComponent<Button>().onClick.AddListener(() => osh.activatePlacingTool(objectPrefabs[i]));
            objectButtons.Add(temp);
        } */
    }

    // Update is called once per frame
    void Update () {
	
	}

    void initializeMaterialView()
    {
        groundMaterials.Clear();
        groundMaterialsInverse.Clear();
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        foreach (Material mat in Resources.LoadAll<Material>(Constants.GROUND_MATERIAL_PATH))
        {
            string name = mat.name;
            //groundMaterials.Add(name, mat);
            groundMaterialsInverse.Add(mat, name);
        }

        materials = new List<Material>(groundMaterialsInverse.Keys);


        int counter = 0;
        float startX = 0;
        //materialView = GameObject.Find("MaterialView");
        RectTransform materialRect = materialView.GetComponent<RectTransform>();
        materialRect.sizeDelta = new Vector2(materials.Count * 800, 0);


        foreach (Material key in materials)
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
            GameObject parent = GameObject.Find("MaterialBar");
            temp.transform.SetParent(parent.transform);
            materialButtons.Add(temp);
            Debug.Log(counter);
            temp.AddComponent<Button>().onClick.AddListener(() =>
            {
                osh.activateBrushTool(materials[materialButtons.IndexOf(temp)]);
                Debug.Log("Uebermitteltes Asset: " + temp.name);
            });
            if (counter == 0)
                temp.transform.position = new Vector3(startX, 57.5f, 0);
            else
                temp.transform.position = new Vector3(startX + counter * 10, 57.5f, 0);
            temp.name = "Button " + counter;
            startX += 165;
            counter++;
        }
    }

    void initializeObjectView()
    {
        prefabs.Clear();
        prefabsInverse.Clear();
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        foreach (GameObject prefab in Resources.LoadAll<GameObject>(Constants.PROPS_PREFAB_PATH))
        {
            string prefabName = prefab.name;
            prefabs.Add(prefabName, prefab);
            prefabsInverse.Add(prefab, prefabName);
        }

        objectPrefabs = new List<GameObject>(prefabsInverse.Keys);


        int counter = 0;
        float startX = 0;
        
        RectTransform objectRect = objectView.GetComponent<RectTransform>();
        objectRect.sizeDelta = new Vector2(objectPrefabs.Count * 800, 0);


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
            temp.transform.SetParent(parent.transform);
            objectButtons.Add(temp);
            temp.AddComponent<Button>().onClick.AddListener(() =>
            {
                osh.activatePlacingTool(objectPrefabs[objectButtons.IndexOf(temp)]);
                Debug.Log("Uebermitteltes Objekt: " + key.name);
            });
            if (counter == 0)
                temp.transform.position = new Vector3(startX, 57.5f, 0);
            else
                temp.transform.position = new Vector3(startX + counter * 10, 57.5f, 0);
            temp.name = "Button " + counter;
            startX += 165;
            counter++;
        }
    }



    void showAssetBar(string viewName)
    {
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
    }
    

}
