using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEditor;


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

    public void handleCreate()
    {
        int index = drop.value;


        LevelConfiguration.instance.defaultValues = false;
        LevelConfiguration.instance.gridHeight = mapSizeArray[0,index];
        LevelConfiguration.instance.gridWidth = mapSizeArray[1, index];
        LevelConfiguration.instance.filename = GameObject.Find("InputField").GetComponent<InputField>().text;
        if (LevelConfiguration.instance.gridHeight == 20 && LevelConfiguration.instance.gridWidth == 20) {
            SceneManager.LoadScene("EmptyLevel 1");
        }
        else if (LevelConfiguration.instance.gridHeight == 20 && LevelConfiguration.instance.gridWidth == 40) {
            SceneManager.LoadScene("EmptyLevel 2");
        }
        else if (LevelConfiguration.instance.gridHeight == 40 && LevelConfiguration.instance.gridWidth == 80) {
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
            LevelConfiguration peekedHeader = ls.peekHeader(loadingPath);
            if (peekedHeader.gridHeight == 20 && peekedHeader.gridWidth == 20) {
                SceneManager.LoadScene("EmptyLevel 1");
            }
            else if (peekedHeader.gridHeight == 20 && peekedHeader.gridWidth == 40) {
                SceneManager.LoadScene("EmptyLevel 2");
            }
            else if (peekedHeader.gridHeight == 40 && peekedHeader.gridWidth == 80) {
                SceneManager.LoadScene("EmptyLevel 3");
            }
        }
    }
    
    void Destroy()
    {
        createB.onClick.RemoveListener(() => handleCreate());
        loadB.onClick.RemoveListener(() => handleLoad());
    }
}
