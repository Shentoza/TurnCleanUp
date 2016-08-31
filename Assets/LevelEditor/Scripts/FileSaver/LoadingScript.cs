using UnityEngine;
using System.Collections;
using System.IO;

public class LoadingScript : MonoBehaviour {

    LevelConfiguration levelConfig;
    private BinaryReader m_reader;

    public string filePath;
    public bool editorMode;
    
    private ObjectSetterLE m_objectSetterLE;
    private BattlefieldCreatorLE m_battlefieldCreator;

    private GameObject m_currentPrefab;
    private GameObject m_currentGameObject;
    private Constants.FILE_OBJECT_FLAGS m_currentObjectFlag;
    private Constants.FILE_COMPONENT_FLAGS m_currentComponentFlag;

    public void OnLevelWasLoaded()
    {
        if (string.IsNullOrEmpty(filePath))
            return;
        loadLevel(filePath);
        filePath = null;
    }

    public void loadLevel(string path)
    {
        levelConfig = LevelConfiguration.instance;

        m_objectSetterLE = FindObjectOfType<ObjectSetterLE>();
        if (m_objectSetterLE == null) {
            Debug.Log("Object Setter ist null!");
            return;
        }

        m_battlefieldCreator = FindObjectOfType<BattlefieldCreatorLE>();
        if(m_battlefieldCreator == null) {
            Debug.Log("Battlefield Creator ist null!");
            return;
        }


        try
        {
            m_reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log(ex.Message);
            ex.ToString();
        }
        bool reading = readHeader();
        if (!reading)
            Debug.Log("Falscher Header!");


        readCellTextures();
        while (reading)
        {
            try
            {
                m_currentObjectFlag = (Constants.FILE_OBJECT_FLAGS)m_reader.ReadInt16();
                switch(m_currentObjectFlag) { 
                    case Constants.FILE_OBJECT_FLAGS.NewObject: {
                        readObject();
                        break;
                    }
                    case Constants.FILE_OBJECT_FLAGS.EndOfFile: {
                        reading = false;
                        break;
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
                reading = false;
                ex.ToString();
            }
        }
        m_reader.BaseStream.Close();
        m_reader.Close();
    }

    private bool readHeader()
    {
        string beginning = m_reader.ReadString();
        if (!beginning.Equals(Constants.FILE_BEGINNING_TAG)) {
            Debug.Log("Wrong File Start");
            //Falscher File Start
            return false;
        }
        readLevelConfig(levelConfig);
        readSpawnpoints();
        
        if(m_reader.ReadString().Equals(Constants.FILE_END_OF_HEADER))
            return true;
        return false;
    }

    private void readLevelConfig(LevelConfiguration config)
    {
        config.defaultValues = m_reader.ReadBoolean();
        if (config.defaultValues) {
            config.gridWidth = Constants.DEFAULT_GRID_WIDTH;
            config.gridHeight = Constants.DEFAULT_GRID_HEIGHT;
            config.objectCount = Constants.DEFAULT_OBJECT_COUNT;
            config.cubeMaterial = Constants.DEFAULT_CUBE_MATERIAL;
        }
        else {
            config.gridWidth = m_reader.ReadInt32();
            config.gridHeight = m_reader.ReadInt32();
            config.objectCount = m_reader.ReadInt32();
            //levelConfig.cubeMaterial = LookUpTable.materials[m_reader.ReadString()];
        }
    }

    private void readSpawnpoints()
    {
        System.Collections.Generic.List<Vector2> p1;
        System.Collections.Generic.List<Vector2> p2;
        if (editorMode) {
            BattlefieldCreatorLE bfle = FindObjectOfType<BattlefieldCreatorLE>();
            p1 = bfle.startPostionsP1;
            p2 = bfle.startPostionsP2;
        }
        else {
            BattlefieldCreater bc = FindObjectOfType<BattlefieldCreater>();
            p1 = bc.startPostionsP1;
            p2 = bc.startPostionsP2;
        }

        int count = m_reader.ReadInt32();
        for (int i = 0; i < count; ++i)
            p1.Add(new Vector2(m_reader.ReadSingle(), m_reader.ReadSingle()));

        count = m_reader.ReadInt32();
        for (int i = 0; i < count; ++i)
            p2.Add(new Vector2(m_reader.ReadSingle(), m_reader.ReadSingle()));
    }

    private void readCellTextures()
    {

        if (editorMode) {
            BattlefieldCreatorLE bfle = FindObjectOfType<BattlefieldCreatorLE>();
            MeshRenderer currentMR;
            for (int z = 0; z < bfle.sizeZ * 10; ++z) { 
                for(int x = 0; x < bfle.sizeX * 10; ++x) {
                    currentMR = bfle.Farbzellen[x, z].GetComponent<MeshRenderer>();
                    string matstr = m_reader.ReadString();
                    currentMR.material = LookUpTable.materials[matstr];
                }
            }
            m_reader.ReadString();
        }

        //Nicht im Editor Modus -> keine Farbzellen -> lies solange aus, bis du am Ende bist.
        else {
            string tmp = m_reader.ReadString();
            while (tmp != Constants.FILE_END_OF_TEXTURE_CELLS)
                tmp = m_reader.ReadString();
        }
    }

    private void readObjectHeader()
    {
        string prefabTag = m_reader.ReadString();
        if(!prefabTag.Equals(Constants.FILE_NO_PREFAB_TAG)) {
            m_currentPrefab = LookUpTable.prefabs[prefabTag];
            m_currentGameObject = Instantiate<GameObject>(m_currentPrefab);
        }
        else {
            m_currentGameObject = new GameObject();
        }
        m_currentGameObject.name = m_reader.ReadString();
        m_currentGameObject.tag = m_reader.ReadString();
    }

    private void readObject(GameObject parentObject = null)
    {
        readObjectHeader();
        readTransform(parentObject);
        do
        {
            m_currentComponentFlag = (Constants.FILE_COMPONENT_FLAGS) m_reader.ReadInt32();
            switch (m_currentComponentFlag)
            {
                case Constants.FILE_COMPONENT_FLAGS.ObjectComponent: {
                        readObjectComponent();
                        break;
                    }
                case Constants.FILE_COMPONENT_FLAGS.ObjectSetter: {
                        readObjectSetter();
                        break;
                    }
                /*
                //KindObjekt gefunden, sollte letztes Komponentenflag sein
                case Constants.FILE_COMPONENT_FLAGS.ChildObject: {
                        GameObject parent = m_currentGameObject;
                        m_currentGameObject = new GameObject();
                        readObject(parent);
                        m_currentGameObject = parent;
                        break;
                    }
                 */
                case Constants.FILE_COMPONENT_FLAGS.EndOfObject:
                    break;

                default:
                    break;
            }
        } while (m_currentComponentFlag != Constants.FILE_COMPONENT_FLAGS.EndOfObject);

        if(!editorMode)
            foreach(string tag in Constants.IGNORE_IN_PLAY_MODE_TAGS) {
                if (m_currentGameObject.tag.Equals(tag))
                    Destroy(m_currentGameObject);
            }
        else {
            ObjectSetter objSet = m_currentGameObject.GetComponent<ObjectSetter>();
            if(objSet != null)
                m_objectSetterLE.moveObject(m_battlefieldCreator.getZellen(), objSet.x, objSet.z, m_currentGameObject, true);
        }
    }

    private void readTransform(GameObject parentObject = null)
    {
        if (parentObject != null)
            m_currentGameObject.transform.SetParent(parentObject.transform);

        m_currentGameObject.transform.position = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.rotation = new Quaternion(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
        m_currentGameObject.transform.localScale = new Vector3(m_reader.ReadSingle(), m_reader.ReadSingle(), m_reader.ReadSingle());
    }

    private void readObjectSetter()
    {
        ObjectSetter defaultObjectSetter = m_currentGameObject.GetComponent<ObjectSetter>();
        if(null != defaultObjectSetter) {
            defaultObjectSetter.x = m_reader.ReadInt32();
            defaultObjectSetter.z = m_reader.ReadInt32();
        }
    }

    private void readObjectComponent()
    {
        ObjectComponent defaultObjectComponent = m_currentGameObject.GetComponent<ObjectComponent>();
        if(null != defaultObjectComponent) {
            defaultObjectComponent.sizeX = m_reader.ReadInt32();
            defaultObjectComponent.sizeZ = m_reader.ReadInt32();
            defaultObjectComponent.original = m_currentPrefab;
        }
    }

    public LevelConfiguration peekHeader(string path)
    {

        try {
            m_reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }
        catch (FileNotFoundException ex) {
            Debug.Log(ex.Message);
            ex.ToString();
        }

        if (!m_reader.ReadString().Equals(Constants.FILE_BEGINNING_TAG)) {
            Debug.Log("Wrong File Start");
            //Falscher File Start
            return null;
        }
        LevelConfiguration result = new LevelConfiguration(0, 0);
        readLevelConfig(result);

        m_reader.BaseStream.Close();
        m_reader.Close();
        return result;
    }

}
