using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MenuScript : MonoBehaviour {

    public Canvas settingsCanvas;
    public Canvas quitCanvas;

    public Text playButton;
    public Text settingsButton;
    public Text quitButton;

    public Text currentVolume;
    public Slider volumeSlider;
    public Dropdown resolutionDropdrown;
    public Text okay;

    public Text yes;
    public Text no;

	// Use this for initialization
	void Start ()
    {
        settingsCanvas = settingsCanvas.GetComponent<Canvas>();
        quitCanvas = quitCanvas.GetComponent<Canvas>();
        playButton = playButton.GetComponent<Text>();
        quitButton = quitButton.GetComponent<Text>();
        currentVolume = currentVolume.GetComponent<Text>();
        //volumeSlider = quitButton.GetComponent<Slider>();
        resolutionDropdrown = resolutionDropdrown.GetComponent<Dropdown>();
        okay = okay.GetComponent<Text>();
        yes = yes.GetComponent<Text>();
        no = no.GetComponent<Text>();
        settingsCanvas.enabled = false;
        quitCanvas.enabled = false;
        currentVolume.text = volumeSlider.value + "";
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void settingsPressed()
    {
        settingsCanvas.enabled = true;
        disableDefaultButtons();
    }

    public void quitPressed()
    {
        quitCanvas.enabled = true;
        disableDefaultButtons();
    }

    public void disableDefaultButtons()
    {
        playButton.enabled = false;
        settingsButton.enabled = false;
        quitButton.enabled = false;
    }

    public void enableDefaultButtons()
    {
        playButton.enabled = true;
        settingsButton.enabled = true;
        quitButton.enabled = true;
        settingsCanvas.enabled = false;
        quitCanvas.enabled = false;
    }

    public void startGame()
    {
        SceneManager.LoadScene("FinalMapScene");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void sliderChanged()
    {
        currentVolume.text = Math.Round(volumeSlider.value, 2)+"";
        //AudioManager.changeVolume(volumeSlider.value);
    }

    public void changeResolution()
    {
        if(resolutionDropdrown.value == 0)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else if (resolutionDropdrown.value == 1)
        {
            Screen.SetResolution(1600, 900, true);
        }
        else if (resolutionDropdrown.value == 2)
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }
}
