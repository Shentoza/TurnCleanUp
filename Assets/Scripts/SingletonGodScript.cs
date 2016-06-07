using UnityEngine;
using System.Collections;

public class SingletonGodScript : MonoBehaviour {
    [SerializeField]
    private PlayerAssistanceSystem prefab;


	// Use this for initialization
	void Start () {
        PlayerAssistanceSystem test = Instantiate(prefab.GetComponent<PlayerAssistanceSystem>());
        test.transform.SetParent(this.transform);
	}

}
