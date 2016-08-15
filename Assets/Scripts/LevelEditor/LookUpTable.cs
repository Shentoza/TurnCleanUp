using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookUpTable : MonoBehaviour {
    public Dictionary<Constants.PREFAB_FLAGS, GameObject> prefabs
        = new Dictionary<Constants.PREFAB_FLAGS, GameObject>();

    public void Start() {
        prefabs.Add(Constants.PREFAB_FLAGS.None, null);
        prefabs.Add(Constants.PREFAB_FLAGS.Barrel_LP, Resources.Load<GameObject>(Constants.PROPS_PREFAB_PATH + "barrel2_lp.prefab");
    }
}
