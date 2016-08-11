using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class SavingScript : MonoBehaviour {

    LevelConfiguration levelConfig;
    private BinaryWriter m_writer;
    List<GameObject> savedGameObjects = new List<GameObject>();

    public void saveLevel(string path)
    {
        levelConfig = LevelConfiguration.instance;
        if(!path.EndsWith(Constants.FILE_EXTENSION))
        {
            path += Constants.FILE_EXTENSION;
        }

        m_writer = new BinaryWriter(new FileStream(path, FileMode.Create));
        savedGameObjects.Clear();

        levelConfig.objectCount = 0;
        foreach(Transform t in FindObjectsOfType<Transform>())
        {
            bool correctTag = false;
            foreach(string tag in Constants.LEVEL_ITEM_TAGS)
            {
                if(t.tag.Equals(tag)) {
                    correctTag = true;
                    break;
                }
            }

            if(correctTag) {
                levelConfig.objectCount++;
                savedGameObjects.Add(t.gameObject);
            }
        }
        writeHeader();

        foreach(GameObject currentGameObject in savedGameObjects)
        {
            //Flag, dass ein neues Objekt kommt
            m_writer.Write((short)Constants.OBJECT_FLAGS.NewObject);
            writeObject(currentGameObject);
        }
        //Flag dass Ende des Files erreicht ist
        m_writer.Write((short)Constants.OBJECT_FLAGS.EndOfFile);

        m_writer.Close();
    }

    public void writeHeader()
    {
        m_writer.Write(Constants.MAP_FILE_BEGINNING);
        m_writer.Write(levelConfig.defaultValues);
        //Default Values benutzt
        if (levelConfig.defaultValues)
            return;

        m_writer.Write(levelConfig.gridWidth);
        m_writer.Write(levelConfig.gridHeight);
        m_writer.Write(levelConfig.objectCount);
    }

    public void writeObject(GameObject gameObject)
    {
        writeObjectHeader(gameObject);
        writeTransform(gameObject.transform);

        //Laufe alle möglichen Components durch, speichere sie, wenn keine Components mehr vorhanden sind, setze EndOfObjectFlag
        for(int i = 1; i < (int)Constants.COMPONENT_FLAGS.Count; ++i) {
            //TODO: Komponenten ausdefinieren
            Constants.COMPONENT_FLAGS componentFlag = (Constants.COMPONENT_FLAGS) i;
            switch (componentFlag) {
                case Constants.COMPONENT_FLAGS.Cell:
                    {
                        break;
                    }
                case Constants.COMPONENT_FLAGS.EndOfObject:
                    {
                        break;
                    }
                default:
                    continue;
            }
        }

        //Überprüfen ob GameObjekt Kindobjekte hat
        for(int i = 0; i < gameObject.transform.childCount; ++i) {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            m_writer.Write((int)Constants.COMPONENT_FLAGS.ChildObject);
            writeObject(childObject);
        }
        m_writer.Write((int)Constants.COMPONENT_FLAGS.EndOfObject);
        savedGameObjects.Remove(gameObject);
    }

    public void writeObjectHeader(GameObject gameObject)
    {
        m_writer.Write(gameObject.name);
        m_writer.Write(gameObject.tag);
    }

    public void writeTransform(Transform transform)
    {
        m_writer.Write(transform.position.x);
        m_writer.Write(transform.position.y);
        m_writer.Write(transform.position.z);
        m_writer.Write(transform.rotation.x);
        m_writer.Write(transform.rotation.y);
        m_writer.Write(transform.rotation.z);
        m_writer.Write(transform.rotation.w);
        m_writer.Write(transform.localScale.x);
        m_writer.Write(transform.localScale.y);
        m_writer.Write(transform.localScale.z);
    }
}
