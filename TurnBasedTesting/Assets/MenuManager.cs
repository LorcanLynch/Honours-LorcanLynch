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

    public Button[] tutButtons;

    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI creditsText;
    public AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        aS.Play();
        SceneManager.LoadScene(1);
    }

    public void Upgradeless()
    {
        aS.Play();
        SceneManager.LoadScene(2);
    }

    public void Controls()
    {
        aS.Play();
        playButton.gameObject.SetActive(false);
        upgradelessButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        foreach(Button button in tutButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void Credits()
    {
        aS.Play();
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
        aS.Play();
        playButton.gameObject.SetActive(true);
        upgradelessButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);
        foreach (Button button in tutButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        aS.Play();
        Application.Quit();
    }


}
