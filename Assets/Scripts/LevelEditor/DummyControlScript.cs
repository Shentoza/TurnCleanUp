using UnityEngine;
using System.Collections;

public class DummyControlScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown("t"))
        {
            SavingScript.saveLevel("test123");
        }

        if(Input.GetKeyDown("l"))
        {
            SavingScript.loadLevel("test123.cwmap");
        }
	}
}
