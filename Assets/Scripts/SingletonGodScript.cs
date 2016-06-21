using UnityEngine;
using System.Collections;

public class SingletonGodScript : MonoBehaviour {

    [SerializeField]
    private GameObject PlayerAssistancePrefab;
    [SerializeField]
    private GameObject ShootingSystemPrefab;

	// Use this for initialization
	void Start () {
        PlayerAssistanceSystem.Initialize(PlayerAssistancePrefab, this.gameObject).transform.SetParent(this.transform);
        ShootingSystem.Initialize(ShootingSystemPrefab, this.gameObject).transform.SetParent(this.transform);
        HealthSystem.Initialize(this.gameObject).transform.SetParent(this.transform);
        AudioManager.Initialize(this.gameObject).transform.SetParent(this.transform);
    }

}
