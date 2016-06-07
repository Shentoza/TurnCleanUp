using UnityEngine;
using System.Collections;

public class SingletonGodScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerAssistanceSystem.Initialize();
        ShootingSystem.Initialize();
        HealthSystem.Initialize();
	}

}
