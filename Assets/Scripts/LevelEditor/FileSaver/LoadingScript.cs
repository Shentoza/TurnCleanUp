using UnityEngine;
using System.Collections;
using System.IO;

public class LoadingScript : MonoBehaviour {

    LevelConfiguration levelConfig;


    public void loadLevel(string path)
    {
        levelConfig = FindObjectOfType<LevelConfiguration>();
        BinaryReader reader;
        try
        {
            reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }
        catch (FileNotFoundException ex)
        {
            return;
        }
        bool reading = readHeader(reader);
        Constants.OBJECT_FLAGS nextObjectFlag;
        while (reading)
        {
            try
            {
                nextObjectFlag = (Constants.OBJECT_FLAGS)reader.ReadInt32();
                switch(nextObjectFlag)
                { 
                    case Constants.OBJECT_FLAGS.NewObject:
                    {
                        break;
                    }
                    case Constants.OBJECT_FLAGS.EndOfFile:
                    {
                        break;
                    }
                }

            }
            catch (EndOfStreamException ex)
            {
                reading = false;
            }
        }
    }

    public bool readHeader(BinaryReader reader)
    {
        string beginning = reader.ReadString();
        if(!beginning.Equals(Constants.MAP_FILE_BEGINNING))
        {
            //Falscher File Start
            return false;
        }
        levelConfig.defaultValues = reader.ReadBoolean();
        if(levelConfig.defaultValues)
        {
            levelConfig.gridWidth = Constants.DEFAULT_GRID_WIDTH;
            levelConfig.gridHeight = Constants.DEFAULT_GRID_HEIGHT;
            levelConfig.objectCount = Constants.DEFAULT_OBJECT_COUNT;
        }
        else
        {
            levelConfig.gridWidth = reader.ReadInt32();
            levelConfig.gridHeight = reader.ReadInt32();
            levelConfig.objectCount = reader.ReadInt32();
        }
        return true;
    }

    public GameObject readObject(BinaryReader reader)
    {
        return null;
    }

    public GameObject readObjectHeader(BinaryReader reader, GameObject gameObject)
    {
        return null;
    }

    public void readTransform(BinaryReader reader, GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        gameObject.transform.rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        gameObject.transform.localScale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

    }
}
