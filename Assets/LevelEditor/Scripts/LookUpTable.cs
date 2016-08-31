﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookUpTable : MonoBehaviour {
    public static Dictionary<string, GameObject> prefabs
        = new Dictionary<string, GameObject>();

    public static Dictionary<GameObject, string> prefabsInverse 
        = new Dictionary<GameObject, string>();


    public static Dictionary<string, Material> materials
        = new Dictionary<string, Material>();

    public static Dictionary<Material, string> materialsInverse
        = new Dictionary<Material, string>();

    public static Dictionary<Material, string> groundMaterials
        = new Dictionary<Material, string>();

    public static Dictionary<Material, string> groundMaterialsInverse
        = new Dictionary<Material, string>();


    public void Awake()
    {
        //Lädt alle Leveleditor Prefabs aus Constants.PROPS_PREFAB_PATH und legt sie im Directory mit <Name, Gameobjekt> Key-Value Paaren ab.
        if (prefabs.Count == 0) {
            foreach (GameObject prefab in Resources.LoadAll<GameObject>(Constants.PROPS_PREFAB_PATH)) {
                string prefabName = prefab.name;
                prefabs.Add(prefabName, prefab);
                prefabsInverse.Add(prefab, prefabName);
            }
        }   
        
        //Lädt alle Materials
        if(materials.Count == 0)
            foreach(Material mat in Resources.LoadAll<Material>(Constants.MATERIAL_PREFAB_PATH)) {
                string materialName = mat.name;
                materials.Add(materialName, mat);
                materialsInverse.Add(mat, materialName);
            }
    }
}