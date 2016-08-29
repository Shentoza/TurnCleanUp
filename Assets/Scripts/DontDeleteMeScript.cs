using UnityEngine;
using System.Collections;

public class DontDeleteMeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Object.DontDestroyOnLoad(this.gameObject);
	}
}
