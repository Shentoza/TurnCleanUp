using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEditor;


public class MainMenu : MonoBehaviour
{
    //[DllImport("user32.dll")]
    //private static extern void OpenFileDialog(); //in your case : OpenFileDialog
    Dropdown drop;
    Button createB, loadB;
    //1. Dimension Höhe - 2. Dimension Breite
    int[,] mapSizeArray = { { 20, 20, 40 }, { 20, 40, 80 } };

    // Use this for initialization
    void Start()
    {
        drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        drop.ClearOptions();
        drop.options.Add(new Dropdown.OptionData("Klein / 20 x 20"));
        drop.options.Add(new Dropdown.OptionData("Mittel / 20 X 40"));
        drop.options.Add(new Dropdown.OptionData("Groß / 40 x 80"));

        createB = GameObject.Find("CreateB").GetComponent<Button>();
        createB.onClick.AddListener(() => handleCreate());

        loadB = GameObject.Find("LoadB").GetComponent<Button>();
        loadB.onClick.AddListener(() => handleLoad());
    }
    void OnGUI()
    {

    }

    public void handleCreate()
    {
        int index = drop.value;

       // Debug.Log("Dateiname: " + filename + "\n" + "Tageszeit: " + daytime + "\n" + "Mapgröße: " + mapsize);

        LevelConfiguration.instance.gridHeight = mapSizeArray[0,index];
        LevelConfiguration.instance.gridWidth = mapSizeArray[1, index];
        LevelConfiguration.instance.daytime = checkDaytime();
        LevelConfiguration.instance.filename = GameObject.Find("InputField").GetComponent<InputField>().text;
        if(index == 0)
        {
            SceneManager.LoadScene("EmptyLevel 1");
        }
        else if(index == 1)
        {
            SceneManager.LoadScene("EmptyLevel 2");
        }
        else
        {
            SceneManager.LoadScene("EmptyLevel 3");
        }
    }


    public void handleLoad()
    {
        string loadingPath = DialogManager.OpenFileDialog();
        if(loadingPath != null && !loadingPath.Equals(string.Empty)) {
            LoadingScript ls = FindObjectOfType<LoadingScript>();
            ls.filePath = loadingPath;
            ls.editorMode = true;

            SceneManager.LoadScene("EmptyLevel 1");
        }
    }

    public string checkDaytime()
    {
        Toggle dusk = GameObject.Find("DuskT").GetComponent<Toggle>();
        Toggle day = GameObject.Find("DayT").GetComponent<Toggle>();
        Toggle night = GameObject.Find("NightT").GetComponent<Toggle>();

        if (dusk.isOn)
            return "dusk";

        else if (day.isOn)
            return "day";

        else if (night.isOn)
            return "night";

        return null;
    }

    void Destroy()
    {
        createB.onClick.RemoveListener(() => handleCreate());
        loadB.onClick.RemoveListener(() => handleLoad());
    }
}
