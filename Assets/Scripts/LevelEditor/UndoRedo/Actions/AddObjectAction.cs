using UnityEngine;
using System.Collections;
using System;

public class AddObjectAction : URAction
{
    GameObject savedInstance;
    GameObject currentInstance;
    string oldTag;

    public AddObjectAction(GameObject gameObject)
    {
        currentInstance = gameObject;
        savedInstance = GameObject.Instantiate<GameObject>(gameObject);
        oldTag = gameObject.tag;
        savedInstance.name = gameObject.name;
        savedInstance.SetActive(false);
        savedInstance.tag = Constants.UNDO_REDO_TAG;
    }

    public void Delete()
    {
        GameObject.Destroy(savedInstance);
    }

    public void Redo()
    {
        currentInstance = GameObject.Instantiate<GameObject>(savedInstance);
        currentInstance.SetActive(true);
        currentInstance.tag = oldTag;
        currentInstance.name = savedInstance.name;

        ObjectSetterLE objLE = GameObject.FindObjectOfType<ObjectSetterLE>();
        BattlefieldCreatorLE bfle = GameObject.FindObjectOfType<BattlefieldCreatorLE>();
        ObjectComponent objComp = currentInstance.GetComponent<ObjectComponent>();

        objLE.moveObject(bfle.getZellen(), objComp.cell.xCoord, objComp.cell.zCoord, savedInstance, true);
    }

    public void Undo()
    {
        ObjectSetterHelperLE.DestroyObject(currentInstance);
    }
}