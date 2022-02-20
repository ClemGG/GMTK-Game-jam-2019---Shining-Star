using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainPanel, controlsPanel;


    private void Start()
    {
        mainPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }


    public void Play()
    {
        SceneFader.instance.FadeToScene(1);
    }

    public void ToogleControlsPanel(bool b)
    {
        mainPanel.SetActive(!b);
        controlsPanel.SetActive(b);
    }

    public void Quit()
    {
        SceneFader.instance.FadeToQuitScene();
    }

    
}
