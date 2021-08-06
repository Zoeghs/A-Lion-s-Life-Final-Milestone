using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Access to all canvases
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject controlsCanvas;
    [SerializeField] GameObject creditsCanvas;

    void Start()
    {
        // Set main menu as default
        SwitchToMain();

    }

    public void SwitchToMain()
    {
        mainCanvas.SetActive(true);

        controlsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void SwitchToControls()
    {
        controlsCanvas.SetActive(true);

        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void SwitchToCredits()
    {
        creditsCanvas.SetActive(true);

        mainCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        // Load game scene
        SceneManager.LoadScene("Demo");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
