using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class UIEditorManager : MonoBehaviour {

    Button unitB, objectB, brushB, leftRotateB, rightRotateB, removeB;
    GameObject assetBar, objectBar;
    List<GameObject> objectButtons = new List<GameObject>();
    List<GameObject> objectPrefabs;

    // Use this for initialization
    void Start () {
        ObjectSetterHelperLE osh = GameObject.Find("Plane").GetComponent<ObjectSetterHelperLE>();
       // unitB = GameObject.Find("UnitB").GetComponent<Button>();
       // unitB.onClick.AddListener(() => Methode());

        objectB = GameObject.Find("ObjectB").GetComponent<Button>();
        objectB.onClick.AddListener(() => showAssetBar(1));

        brushB = GameObject.Find("BrushB").GetComponent<Button>();
       // brushB.onClick.AddListener(() => );

        leftRotateB = GameObject.Find("RotateLeftB").GetComponent<Button>();
        leftRotateB.onClick.AddListener(() => osh.rotateLeft());

        rightRotateB = GameObject.Find("RotateRightB").GetComponent<Button>();
        rightRotateB.onClick.AddListener(() => osh.rotateRight());

        removeB = GameObject.Find("RemoveB").GetComponent<Button>();
        removeB.onClick.AddListener(() => osh.activateDestroyTool());

        
        objectPrefabs = new List<GameObject>(LookUpTable.prefabsInverse.Keys);
        Debug.Log(LookUpTable.prefabsInverse.Count);
        int counter = 0;
        float startX = 50;

        foreach(GameObject key in objectPrefabs)
        {
            Debug.Log("HURENSOHN");
            //Erstelle neues GameObject für Button
            GameObject temp = new GameObject();
            //Füge ImageComponent hinzu
            temp.AddComponent<Image>();
            //Lese Bild aus Preview aus
            Texture2D texture = AssetPreview.GetAssetPreview(key);
            //Konvertiere Textur zu Sprite
            Sprite textureSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //Füge Textur hinzu
            temp.GetComponent<Image>().sprite = textureSprite;
            //Passe Größe an
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            GameObject parent = GameObject.Find("ObjectBar");
            temp.transform.SetParent(parent.transform);
            objectButtons.Add(temp);
            Debug.Log(counter);
            temp.AddComponent<Button>().onClick.AddListener(() =>
            {
                osh.activatePlacingTool(objectPrefabs[objectButtons.IndexOf(temp)]);
            });
            if (counter == 0)
                temp.transform.position = new Vector3(startX, 57.5f, 0);
            else
                temp.transform.position = new Vector3(startX + counter * 10, 57.5f, 0);
            temp.name = "Button " + counter;
            startX += 165;
            counter++;       
        }


        /*objectBar = GameObject.Find("ObjectBar");

        for (int i = 0; i < objectBar.transform.childCount; i++)
        {
            GameObject temp = objectBar.transform.GetChild(i).gameObject;
            temp.GetComponent<Button>().onClick.AddListener(() => osh.activatePlacingTool(objectPrefabs[i]));
            objectButtons.Add(temp);
        } */

        assetBar = GameObject.Find("Assetbar");
        assetBar.SetActive(true);


    }

    // Update is called once per frame
    void Update () {
	
	}


    void showAssetBar(int value)
    {
        /*if (!assetBar.activeSelf)
        {
            assetBar.SetActive(true);

        }*/
    } 


}
