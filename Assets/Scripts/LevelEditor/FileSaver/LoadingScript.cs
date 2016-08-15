using UnityEngine;
using System.Collections;
using System.IO;

public class LoadingScript : MonoBehaviour {

    LevelConfiguration levelConfig;
    private BinaryReader m_reader;


    private GameObject m_currentGameObject;
    private Constants.OBJECT_FLAGS m_currentObjectFlag;
    private Constants.COMPONENT_FLAGS m_currentComponentFlag;


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
        while (reading)
        {
            try
            {
                m_currentObjectFlag = (Constants.OBJECT_FLAGS)m_reader.ReadInt16();
                switch(m_currentObjectFlag) { 
                    case Constants.OBJECT_FLAGS.NewObject: {
                        m_currentGameObject = new GameObject();
                        readObject();
                        break;
                    }
                    case Constants.OBJECT_FLAGS.EndOfFile: {
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
        do
        {
            m_currentComponentFlag = (Constants.COMPONENT_FLAGS) m_reader.ReadInt32();
            switch (m_currentComponentFlag)
            {
                case Constants.COMPONENT_FLAGS.ObjectComponent: {
                        break;
                    }
                case Constants.COMPONENT_FLAGS.ObjectSetter: {
                        readObjectSetter();
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
        } while (m_currentComponentFlag != Constants.COMPONENT_FLAGS.EndOfObject);
    }

    public void readTransform(GameObject parentObject = null)
    {
        if (parentObject != null)
            m_currentGameObject.transform.SetParent(parentObject.transform);

        m_currentGameObject.transform.position = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.rotation = new Quaternion(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.localScale = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
    }

    public void readObjectSetter()
    {
        ObjectSetter defaultObjectSetter = m_currentGameObject.GetComponent<ObjectSetter>();
        if(!defaultObjectSetter.Equals(null)) {
            defaultObjectSetter.x = m_reader.ReadInt32();
            defaultObjectSetter.z = m_reader.ReadInt32();
        }
    }

    public void readObjectComponent()
    {
        ObjectComponent defaultObjectComponent = m_currentGameObject.GetComponent<ObjectComponent>();
        if(!defaultObjectComponent.Equals(null)) {
            defaultObjectComponent.sizeX = m_reader.ReadInt32();
            defaultObjectComponent.sizeZ = m_reader.ReadInt32();
        }
    }
}
