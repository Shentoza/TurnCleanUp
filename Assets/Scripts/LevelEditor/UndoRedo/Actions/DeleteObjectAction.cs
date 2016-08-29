using UnityEngine;
using System.Collections;
using System;

public class DeleteObjectAction : URAction
{
    private GameObject savedInstance;
    private GameObject currentInstance;
    private string oldTag;


    public DeleteObjectAction(GameObject deletedObject)
    {
        savedInstance = GameObject.Instantiate<GameObject>(deletedObject);
        oldTag = deletedObject.tag;
        savedInstance.name = deletedObject.name;
        savedInstance.SetActive(false);
        savedInstance.tag = Constants.UNDO_REDO_TAG;
    }


    public void Delete()
    {
        GameObject.Destroy(savedInstance);
    }

    public void Redo()
    {
        ObjectSetterHelperLE.DestroyObject(currentInstance);
    }

    public void Undo()
    {
        currentInstance = GameObject.Instantiate<GameObject>(savedInstance);
        currentInstance.SetActive(true);
        currentInstance.name = savedInstance.name;
        currentInstance.tag = oldTag;

        ObjectSetterLE objLE = GameObject.FindObjectOfType<ObjectSetterLE>();
        BattlefieldCreatorLE bfle = GameObject.FindObjectOfType<BattlefieldCreatorLE>();
        ObjectComponent objComp = currentInstance.GetComponent<ObjectComponent>();


        objLE.moveObject(bfle.getZellen(), objComp.cell.xCoord, objComp.cell.zCoord, savedInstance, true);
    }
}
