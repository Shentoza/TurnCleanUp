using UnityEngine;
using System.Collections;
using System;

public class AddObjectAction : URAction
{
    GameObject addedObject;
    GameObject savedInstance;

    public AddObjectAction(GameObject gameObject)
    {
        addedObject = gameObject;
    }

    public void delete()
    {
        GameObject.Destroy(savedInstance);
    }

    public void redo()
    {
    }

    public void undo()
    {
        savedInstance = GameObject.Instantiate<GameObject>(addedObject);
        savedInstance.tag = Constants.UNDO_REDO_TAG;
        savedInstance.SetActive(false);
        
    }
}
