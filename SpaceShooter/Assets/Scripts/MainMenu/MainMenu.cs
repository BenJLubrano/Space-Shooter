using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string playScene;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void PlayGame()
    {
        gameManager.BeginIntroSequence();
    }

    public void LoadOtherScene(string sceneName)
    {
        gameManager.SceneTransition(1f, sceneName, "InPlay", false, true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowMain()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ChangeMusicVolume(Slider slider)
    {
        gameManager.SetMusicVolume(slider.value);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        gameManager.SetSFXVolume(slider.value);
    }
}
