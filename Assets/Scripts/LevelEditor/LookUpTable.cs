﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookUpTable : MonoBehaviour {
    public static Dictionary<string, GameObject> prefabs
        = new Dictionary<string, GameObject>();

    public static Dictionary<GameObject, string> prefabsInverse 
        = new Dictionary<GameObject, string>();



    public void Start()
    {
        prefabs.Clear();
        prefabsInverse.Clear();
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        foreach (GameObject prefab in Resources.LoadAll<GameObject>(Constants.PROPS_PREFAB_PATH)) {
            string prefabName = prefab.name;
            prefabs.Add(prefabName, prefab);
            prefabsInverse.Add(prefab, prefabName);
        }
    }
}
