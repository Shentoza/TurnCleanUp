using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEditorManager : MonoBehaviour {

    Button unitB, objectB, brushB, leftRotateB, rightRotateB, removeB;

	// Use this for initialization
	void Start () {
        unitB = GameObject.Find("UnitB").GetComponent<Button>();
        unitB.onClick.AddListener(() => Methode());

        objectB = GameObject.Find("ObjectB").GetComponent<Button>();
        objectB.onClick.AddListener(() => Methode());

        brushB = GameObject.Find("BrushB").GetComponent<Button>();
        brushB.onClick.AddListener(() => Methode());

        leftRotateB = GameObject.Find("RotateLeftB").GetComponent<Button>();
        leftRotateB.onClick.AddListener(() => Methode());

        rightRotateB = GameObject.Find("RotateRightB").GetComponent<Button>();
        rightRotateB.onClick.AddListener(() => Methode());

        removeB = GameObject.Find("RemoveB").GetComponent<Button>();
        removeB.onClick.AddListener(() => Methode());
    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
