using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource musicSource; 
    private bool isMusicOn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
    }

    private void Start()
    {
        UpdateBGMVolume();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        UpdateBGMVolume();
    }

    private void UpdateBGMVolume()
    {
        if (isMusicOn)
        {
            musicSource.volume = 1f; // 设置正常音量
        }
        else
        {
            musicSource.volume = 0f; // 将音量设置为0
        }
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
}
