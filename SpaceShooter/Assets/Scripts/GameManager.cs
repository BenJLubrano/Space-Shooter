using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int shipId = 0;

    [Header("Music")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource musicPlayer2;
    [SerializeField] AudioClip menuTheme;
    [SerializeField] AudioClip noCombat;
    [SerializeField] AudioClip combat;
    [SerializeField] AudioClip pause;
    [SerializeField] AudioClip introOutro;

    [Header("Boss Tracks")]
    [SerializeField] AudioClip boss1;
    [SerializeField] AudioClip boss2;
    [SerializeField] AudioClip boss3;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Image whiteScreen;
    int primaryPlayer = 0;
    float fadeAmount = 0f;
    float fadeSpeed = 1f;
    bool crossFading = false;
    bool canChangeMusic = true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(crossFading)
        {
            fadeAmount += fadeSpeed * Time.deltaTime;
            //if primary player is 0 we should fade that one in
            if (primaryPlayer == 0)
            {
                musicPlayer.volume = fadeAmount;
                musicPlayer2.volume = 1 - fadeAmount;
            }
            else
            {
                musicPlayer.volume = 1 - fadeAmount;
                musicPlayer2.volume = fadeAmount;
            }
            if(fadeAmount >= 1)
            {
                crossFading = false;
                fadeAmount = 0;
            }
        }
    }

    public void UnlockMusic()
    {
        canChangeMusic = true;
    }

    //called whenever a ShipController is added. The ShipController will get a new ID
    public int RegisterShip()
    {
        shipId += 1;
        return shipId;
    }

    public void SetCombatState(bool inCombat)
    {
        if (inCombat)
        {
            CrossFadeAudio("inCombat", .25f);
        }
        else
        {
            CrossFadeAudio("combat", .25f);
        }
    }

    public void SetMusicVolume(float amt)
    {
        mixer.SetFloat("Music", (1 - amt) * -40);
    }

    public float GetMusicVolume()
    {
        float value;
        mixer.GetFloat("Music", out value);
        return 1 - (value / -40);
    }

    public void SetSFXVolume(float amt)
    {
        mixer.SetFloat("SFX", (1 - amt) * -40);
    }

    public float GetSFXVolume()
    {
        float value;
        mixer.GetFloat("SFX", out value);
        return 1 - (value / -40);
    }

    public void BeginIntroSequence()
    {
        SceneTransition(2, "Intro", "intro");
    }

    public void CrossFadeAudio(string newClip, float fadeSpeed = 1)
    {
        if (!canChangeMusic)
        {
            Debug.LogError("Cannot Transition to: " + newClip + " because the MusicPlayer is locked at this time.");
            return;
        }
        AudioClip fadeInClip = null;

        if(newClip == "intro")
        {
            fadeInClip = introOutro;
        }
        else if (newClip == "inPlay")
        {
            fadeInClip = noCombat;

        }
        else if (newClip == "inCombat")
        {
            fadeInClip = combat;
        }
        else if (newClip == "pause")
        {
            fadeInClip = pause;
        }
        else if (newClip == "menu")
        {
            fadeInClip = menuTheme;
        }
        else if (newClip == "boss1")
        {
            fadeInClip = boss1;
        }
        else if (newClip == "boss2")
        {
            fadeInClip = boss2;
        }
        else if (newClip == "boss3")
        {
            fadeInClip = boss3;
        }



        if (primaryPlayer == 0)
        {
            musicPlayer2.clip = fadeInClip;
            musicPlayer2.volume = 0;
            musicPlayer2.Play();
            primaryPlayer = 1;
        }
        else
        {
            musicPlayer.clip = fadeInClip;
            musicPlayer.volume = 0;
            musicPlayer.Play();
            primaryPlayer = 0;
        }

        crossFading = true;
        this.fadeSpeed = fadeSpeed;
    }

    public void SceneTransition(float transitionTime, string sceneName, string newMusic = null, bool bossFight = false)
    {
        if (sceneName == SceneManager.GetActiveScene().name)
            return;
        StartCoroutine(FadeIn(transitionTime/2, sceneName));
        if (newMusic != null)
        {
            CrossFadeAudio(newMusic, transitionTime);
            if(bossFight)
            {
                canChangeMusic = false;
            }
        }
    }



    IEnumerator FadeIn(float time, string scene)
    {
        Color tempColor = whiteScreen.color;
        float elapsedTime = 0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            tempColor.a = elapsedTime / time;
            whiteScreen.color = tempColor;
            yield return null;
        }
        SceneManager.LoadScene(scene);
        StartCoroutine(FadeOut(time));
    }

    IEnumerator FadeOut(float time)
    {
        Color tempColor = whiteScreen.color;
        float timeLeft = time;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            tempColor.a = timeLeft / time;
            whiteScreen.color = tempColor;
            yield return null;
        }
    }
}
