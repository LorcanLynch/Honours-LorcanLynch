using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    public Button upgradelessButton;
    public Button controlsButton;
    public Button creditsButton;
    public Button quitButton;
    public Button backButton;

    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI creditsText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Upgradeless()
    {
        SceneManager.LoadScene(2);
    }

    public void Controls()
    {
        playButton.gameObject.SetActive(false);
        upgradelessButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        controlsText.gameObject.SetActive(true);
    }

    public void Credits()
    {
        playButton.gameObject.SetActive(false);
        upgradelessButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(true);
    }

    public void Back()
    {
        playButton.gameObject.SetActive(true);
        upgradelessButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }


}
