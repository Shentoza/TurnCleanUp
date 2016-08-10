using UnityEngine;
using System.Collections;
using System.IO;

public class LoadingScript : MonoBehaviour {


    public static void loadLevel(string path)
    {

        int count = 0;
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

        while (reading)
        {
            try
            {
                count = reader.ReadInt32();
                go = new GameObject();
                readTransform(reader, go);
                go.transform.position = trans_pos;
                go.transform.rotation = trans_rot;
                go.transform.localScale = trans_scale;
                go.name = name;
                go.tag = tag;
            }
            catch (EndOfStreamException ex)
            {
                reading = false;
            }
        }
    }

    public static int readHeader(BinaryReader reader)
    {
        return true;
    }

    public static bool readObjectHeader(BinaryReader reader, GameObject gameObject)
    {

    }

    public static void readTransform(BinaryReader reader, GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        gameObject.transform.rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        gameObject.transform.localScale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

    }
}
