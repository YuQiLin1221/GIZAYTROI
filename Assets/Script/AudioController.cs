using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float soundVolume;

    public AudioSource musicAus;
    public AudioSource soundAus;


    public AudioClip[] backgroundMusics;
    public AudioClip[] rightSound;
    public AudioClip[] loseSound;
    public AudioClip[] winSound;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayBackGroundMusic();
    }

    public void PlayBackGroundMusic()
    {
        if (musicAus && backgroundMusics != null && backgroundMusics.Length > 0)
        {
            int randIdx = Random.Range(0, backgroundMusics.Length);
            if (backgroundMusics[randIdx])
            {
                musicAus.clip = backgroundMusics[randIdx];
                musicAus.volume = musicVolume;
                musicAus.Play();
            }
        }
    }

    public void PlaySound(AudioClip sound)
    {
        if(soundAus && sound)
        {
            soundAus.volume = soundVolume;
            soundAus.PlayOneShot(sound);
        }
    }


    public void StopMusic()
    {
        if (musicAus)
        {
            musicAus.Stop();
        }
    }

    public void MakeSingleton()
    {
        if (instance == null) {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
