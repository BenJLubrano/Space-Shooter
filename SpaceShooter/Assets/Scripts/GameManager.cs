using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [SerializeField] AudioMixer mixer;
    [Range(0.0f, 1f)]
    [SerializeField] float outOfCombatVolume;
    [Range(0.0f, 1f)]
    [SerializeField] float inCombatVolume;

    int primaryPlayer = 0;
    float fadeAmount = 0f;
    float fadeSpeed = 1f;
    bool crossFading = false;
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
            musicPlayer.clip = combat;
            musicPlayer.volume = inCombatVolume;
        }
        else
        {
            musicPlayer.clip = noCombat;
            musicPlayer.volume = outOfCombatVolume;
        }
        musicPlayer.Play();
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
        CrossFadeAudio("intro", .5f);
    }

    public void CrossFadeAudio(string newClip, float fadeSpeed = 1)
    {
        AudioClip fadeInClip = noCombat;

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

        if(primaryPlayer == 0)
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
    
}
