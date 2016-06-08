using UnityEngine;
using System.Collections;

public class SingletonGodScript : MonoBehaviour {

    [SerializeField]
    private GameObject PlayerAssistancePrefab;

	// Use this for initialization
	void Start () {
        PlayerAssistanceSystem.Initialize(PlayerAssistancePrefab);
        ShootingSystem.Initialize();
        HealthSystem.Initialize();
	}

}
