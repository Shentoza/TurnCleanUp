using UnityEngine;
using System.Collections;
using System.IO;

public class LoadingScript : MonoBehaviour {

    LevelConfiguration levelConfig;
    private GameObject m_currentGameObject;
    private BinaryReader m_reader;


    public void loadLevel(string path)
    {
        levelConfig = LevelConfiguration.instance;
        try
        {
            m_reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }
        catch (FileNotFoundException ex)
        {
            return;
        }
        bool reading = readHeader();
        Constants.OBJECT_FLAGS nextObjectFlag;
        while (reading)
        {
            try
            {
                nextObjectFlag = (Constants.OBJECT_FLAGS)m_reader.ReadInt16();
                switch(nextObjectFlag)
                { 
                    case Constants.OBJECT_FLAGS.NewObject:
                    {
                        m_currentGameObject = new GameObject();
                        readObject();
                        break;
                    }
                    case Constants.OBJECT_FLAGS.EndOfFile:
                    {
                        reading = false;
                        break;
                    }
                }

            }
            catch (EndOfStreamException ex)
            {
                reading = false;
            }
        }
        m_reader.Close();
    }

    public bool readHeader()
    {
        string beginning = m_reader.ReadString();
        if(!beginning.Equals(Constants.MAP_FILE_BEGINNING))
        {
            Debug.Log("Wrong File Start");
            //Falscher File Start
            return false;
        }
        levelConfig.defaultValues = m_reader.ReadBoolean();
        if(levelConfig.defaultValues)
        {
            levelConfig.gridWidth = Constants.DEFAULT_GRID_WIDTH;
            levelConfig.gridHeight = Constants.DEFAULT_GRID_HEIGHT;
            levelConfig.objectCount = Constants.DEFAULT_OBJECT_COUNT;
        }
        else
        {
            levelConfig.gridWidth = m_reader.ReadInt32();
            levelConfig.gridHeight = m_reader.ReadInt32();
            levelConfig.objectCount = m_reader.ReadInt32();
        }
        return true;
    }

    public void readObjectHeader()
    {
        m_currentGameObject.name = m_reader.ReadString();
        m_currentGameObject.tag = m_reader.ReadString();
    }

    public void readObject(GameObject parentObject = null)
    {
        readObjectHeader();
        readTransform(parentObject);
        Constants.COMPONENT_FLAGS nextComponentFlag = Constants.COMPONENT_FLAGS.EndOfObject;
        do
        {
            nextComponentFlag = (Constants.COMPONENT_FLAGS) m_reader.ReadInt32();
            switch (nextComponentFlag)
            {
                case Constants.COMPONENT_FLAGS.Cell: {
                        break;
                    }
                case Constants.COMPONENT_FLAGS.ObjectSetter: {
                        break;
                    }

                //KindObjekt gefunden, sollte letztes Komponentenflag sein
                case Constants.COMPONENT_FLAGS.ChildObject: {
                        GameObject parent = m_currentGameObject;
                        m_currentGameObject = new GameObject();
                        readObject(parent);
                        m_currentGameObject = parent;
                        break;
                    }
                case Constants.COMPONENT_FLAGS.EndOfObject:
                    break;

                default:
                    break;
            }
        } while (nextComponentFlag != Constants.COMPONENT_FLAGS.EndOfObject);
    }

    public void readTransform(GameObject parentObject = null)
    {
        if (parentObject != null)
            m_currentGameObject.transform.SetParent(parentObject.transform);

        m_currentGameObject.transform.position = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.rotation = new Quaternion(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.localScale = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
    }

    public void readCellComponent()
    {

    }

    public void readObjectSetterComponent()
    {

    }
}
