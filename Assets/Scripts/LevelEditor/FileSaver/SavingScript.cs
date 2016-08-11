using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class SavingScript : MonoBehaviour {

    LevelConfiguration levelConfig;


    public void saveLevel(string path)
    {
        levelConfig = FindObjectOfType<LevelConfiguration>();
        if(!path.EndsWith(Constants.FILE_ENDING))
        {
            path += Constants.FILE_ENDING;
        }

        BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create));
        List<GameObject> savedGameObjects = new List<GameObject>();

        levelConfig.objectCount = 0;
        foreach(Transform t in FindObjectsOfType<Transform>())
        {
            if(Constants.GAME_SETTINGS_TAG.Contains(t.tag))
            {
                savedGameObjects.Add(t.gameObject);
            }
            levelConfig.objectCount++;
        }
        writeHeader(writer);

        foreach(GameObject g in savedGameObjects)
        {
            writeObject(writer, g);
        }

        writer.Close();
    }

    public void writeHeader(BinaryWriter writer)
    {
        writer.Write(Constants.MAP_FILE_BEGINNING);
        writer.Write(levelConfig.defaultValues);
        //Default Values benutzt
        if (levelConfig.defaultValues)
            return;

        writer.Write(levelConfig.gridWidth);
        writer.Write(levelConfig.gridHeight);
        writer.Write(levelConfig.objectCount);
    }

    public void writeObject(BinaryWriter writer, GameObject gameObject)
    {
        writeObjectHeader(writer, gameObject);
        writeTransform(writer, gameObject.transform);
        //Laufe alle möglichen Components durch, speichere sie, wenn keine Components mehr vorhanden sind, setze EndOfObjectFlag
        for(int i = 0; i < (int)Constants.COMPONENT_FLAGS.Count; ++i)
        {
            
        }

    }

    public void writeObjectHeader(BinaryWriter writer, GameObject gameObject)
    {

    }

    public void writeTransform(BinaryWriter writer, Transform transform)
    {
        writer.Write(transform.position.x);
        writer.Write(transform.position.y);
        writer.Write(transform.position.z);
        writer.Write(transform.rotation.x);
        writer.Write(transform.rotation.y);
        writer.Write(transform.rotation.z);
        writer.Write(transform.rotation.w);
        writer.Write(transform.localScale.x);
        writer.Write(transform.localScale.y);
        writer.Write(transform.localScale.z);
    }
}
