using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Main
    public Canvas mainCanvas;
    public Button playButton;
    public Button howtoButton;
    public Button creditsButton;
    // HowTo
    public Canvas howtoCanvas;
    public Button back1Button; // the Back-Button in the HowTo
    // Credits
    public Canvas creditsCanvas;
    public Button back2Button; // the Back-Button in the Credits
    public Button link1Button; // hex-5
    public Button link2Button; // singinwhale
    public Button link3Button; // Seitenbacher
    public Button link4Button; // made3
    public Button link5Button; // alexandertbratrich

    void Awake()
    {
        howtoCanvas.enabled = false;
        creditsCanvas.enabled = false;

        // Main
        playButton.onClick.AddListener(delegate () {
            SceneManager.LoadScene("Default");
        });
        howtoButton.onClick.AddListener(delegate () {
            mainCanvas.enabled = false;
            howtoCanvas.enabled = true;
        });
        creditsButton.onClick.AddListener(delegate () {
            mainCanvas.enabled = false;
            creditsCanvas.enabled = true;
        });
        // HowTo
        back1Button.onClick.AddListener(delegate () {
            howtoCanvas.enabled = false;
            mainCanvas.enabled = true;
        });
        // Credits
        back2Button.onClick.AddListener(delegate () {
            creditsCanvas.enabled = false;
            mainCanvas.enabled = true;
        });
        link1Button.onClick.AddListener(delegate () {
            Application.OpenURL("https://hex-5.itch.io/");
        });
        link2Button.onClick.AddListener(delegate () {
            Application.OpenURL("https://singinwhale.itch.io/");
        });
        link3Button.onClick.AddListener(delegate () {
            Application.OpenURL("https://seitenbacher.itch.io/");
        });
        link4Button.onClick.AddListener(delegate () {
            Application.OpenURL("https://made3.itch.io/");
        });
        link5Button.onClick.AddListener(delegate () {
            Application.OpenURL("https://alexandertbratrich.itch.io/");
        });
    }
}
