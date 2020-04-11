using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Main
    public Canvas mainCanvas;
    private CanvasGroup mainGroup;
    public Button playButton;
    public Button howtoButton;
    public Button creditsButton;
    // HowTo
    public Canvas howtoCanvas;
    private CanvasGroup howtoGroup;
    public Button back1Button; // the Back-Button in the HowTo
    // Credits
    public Canvas creditsCanvas;
    private CanvasGroup creditsGroup;
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

        mainGroup = mainCanvas.transform.GetComponent<CanvasGroup>();
        howtoGroup = howtoCanvas.transform.GetComponent<CanvasGroup>();
        creditsGroup = creditsCanvas.transform.GetComponent<CanvasGroup>();

        mainGroup.alpha = 0;
        howtoGroup.alpha = 0;
        creditsGroup.alpha = 0;

        StartCoroutine("FadeInMain");

        // Main
        playButton.onClick.AddListener(delegate () {
            StartCoroutine("FinalFadeOut");
        });
        howtoButton.onClick.AddListener(delegate () {
            StartCoroutine("FadeOutMain");
            StartCoroutine("FadeInHowTo");
        });
        creditsButton.onClick.AddListener(delegate () {
            StartCoroutine("FadeOutMain");
            StartCoroutine("FadeInCredits");
        });
        // HowTo
        back1Button.onClick.AddListener(delegate () {
            StartCoroutine("FadeOutHowTo");
            StartCoroutine("FadeInMain");
        });
        // Credits
        back2Button.onClick.AddListener(delegate () {
            StartCoroutine("FadeOutCredits");
            StartCoroutine("FadeInMain");
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

    IEnumerator FinalFadeOut()
    {
        while (mainGroup.alpha > 0)
        {
            mainGroup.alpha -= 5f * Time.deltaTime;
            mainCanvas.transform.GetComponent<CanvasGroup>().alpha = mainGroup.alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Default");
        yield break;
    }

    IEnumerator FadeOutMain()
    {
        while (mainGroup.alpha > 0)
        {
            mainGroup.alpha -= 5f * Time.deltaTime;
            mainCanvas.transform.GetComponent<CanvasGroup>().alpha = mainGroup.alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        mainCanvas.enabled = false;
        yield break;
    }

    IEnumerator FadeInMain()
    {
        mainCanvas.enabled = true;
        yield return new WaitForSeconds(0.1f);
        while (mainGroup.alpha < 1)
        {
            mainGroup.alpha += 5f * Time.deltaTime;
            mainCanvas.transform.GetComponent<CanvasGroup>().alpha = mainGroup.alpha;
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeOutHowTo()
    {
        while (howtoGroup.alpha > 0)
        {
            howtoGroup.alpha -= 5f * Time.deltaTime;
            howtoCanvas.transform.GetComponent<CanvasGroup>().alpha = howtoGroup.alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        howtoCanvas.enabled = false;
        yield break;
    }

    IEnumerator FadeInHowTo()
    {
        howtoCanvas.enabled = true;
        yield return new WaitForSeconds(0.1f);
        while (howtoGroup.alpha < 1)
        {
            howtoGroup.alpha += 5f * Time.deltaTime;
            howtoCanvas.transform.GetComponent<CanvasGroup>().alpha = howtoGroup.alpha;
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeOutCredits()
    {
        while (creditsGroup.alpha > 0)
        {
            creditsGroup.alpha -= 5f * Time.deltaTime;
            creditsCanvas.transform.GetComponent<CanvasGroup>().alpha = creditsGroup.alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        creditsCanvas.enabled = false;
        yield break;
    }

    IEnumerator FadeInCredits()
    {
        creditsCanvas.enabled = true;
        yield return new WaitForSeconds(0.1f);
        while (creditsGroup.alpha < 1)
        {
            creditsGroup.alpha += 5f * Time.deltaTime;
            creditsCanvas.transform.GetComponent<CanvasGroup>().alpha = creditsGroup.alpha;
            yield return null;
        }
        yield break;
    }
}
