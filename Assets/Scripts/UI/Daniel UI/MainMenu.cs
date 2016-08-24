using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;


public class MainMenu : MonoBehaviour
{
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
        Dropdown.OptionData selectedItem = drop.options[index];
        string mapsize = selectedItem.text;

       // Debug.Log("Dateiname: " + filename + "\n" + "Tageszeit: " + daytime + "\n" + "Mapgröße: " + mapsize);

        LevelConfiguration.instance.gridHeight = mapSizeArray[0,index];
        LevelConfiguration.instance.gridWidth = mapSizeArray[1, index];
        LevelConfiguration.instance.daytime = checkDaytime();
        LevelConfiguration.instance.filename = GameObject.Find("InputField").GetComponent<InputField>().text;
        SceneManager.LoadScene("LE Scene Builder");
    }

    public void handleLoad()
    {
        /*System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        p.Start(); */
        //Debug.Log("Hallo");
        //System.Windows.Forms.OpenFileDialog sfd = new System.Windows.Forms.OpenFileDialog();
        //OpenFileDialog fileDialog = new OpenFileDialog();
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
