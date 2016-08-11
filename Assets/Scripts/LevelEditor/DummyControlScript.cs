using UnityEngine;
using System.Collections;

public class DummyControlScript : MonoBehaviour {

    [SerializeField]
    private LoadingScript loadingScript;
    [SerializeField]
    private SavingScript savingScript;

	// Use this for initialization
	void Start () {
        loadingScript = FindObjectOfType<LoadingScript>();
        savingScript = FindObjectOfType<SavingScript>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown("t"))
        {
            savingScript.saveLevel("test123");
        }

        if(Input.GetKeyDown("l"))
        {
            loadingScript.loadLevel("test123.cwmap");
        }
	}
}
