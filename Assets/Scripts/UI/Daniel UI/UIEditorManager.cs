using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

        objectPrefabs = new List<GameObject>();

        foreach(GameObject key in LookUpTable.prefabsInverse.Keys)
        {
            objectPrefabs.Add(key);
        }

        objectBar = GameObject.Find("ObjectBar");

        for (int i = 0; i < objectBar.transform.childCount; i++)
        {
            GameObject temp = objectBar.transform.GetChild(i).gameObject;
            temp.GetComponent<Button>().onClick.AddListener(() => osh.activatePlacingTool(objectPrefabs[i]));
            objectButtons.Add(temp);
        }

        assetBar = GameObject.Find("Assetbar");
        assetBar.SetActive(false);


    }

    // Update is called once per frame
    void Update () {
	
	}


    void showAssetBar(int value)
    {
        if (!assetBar.activeSelf)
        {
            assetBar.SetActive(true);

        }
    }


}
