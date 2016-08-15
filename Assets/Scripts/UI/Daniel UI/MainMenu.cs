using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    Dropdown drop;
    Button createB, loadB;

    // Use this for initialization
    void Start()
    {
        drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        drop.ClearOptions();
        drop.options.Add(new Dropdown.OptionData("Klein / 40 x 80"));
        drop.options.Add(new Dropdown.OptionData("Mittel / 80 X 160"));
        drop.options.Add(new Dropdown.OptionData("Groß / 160 x 320"));

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
        string filename = GameObject.Find("InputField").GetComponent<InputField>().text;
        string daytime = checkDaytime();

        Debug.Log("Dateiname: " + filename + "\n" + "Tageszeit: " + daytime);
    }

    public void handleLoad()
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        p.Start();
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
