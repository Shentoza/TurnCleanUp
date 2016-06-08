using UnityEngine;
using System.Collections;

public class SingletonGodScript : MonoBehaviour {

    [SerializeField]
    private GameObject PlayerAssistancePrefab;

	// Use this for initialization
	void Start () {
        PlayerAssistanceSystem.Initialize(PlayerAssistancePrefab, this.gameObject).transform.SetParent(this.transform);
        ShootingSystem.Initialize(this.gameObject).transform.SetParent(this.transform);
        HealthSystem.Initialize(this.gameObject).transform.SetParent(this.transform);
    }

}
