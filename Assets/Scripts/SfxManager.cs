using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;

    public AudioSource sfxSource;
    private bool isSFXOn = true;

    [SerializeField] private AudioClip[] sFx;

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
        isSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
    }

    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
        PlayerPrefs.SetInt("SFXOn", isSFXOn ? 1 : 0); 
    }

    public void PlaySFX(int AudioNum)
    {
        if (isSFXOn)
        {
            sfxSource.PlayOneShot(sFx[AudioNum]); 
        }
    }

    public bool GetIsSfxOn()
    {
        return isSFXOn;
    }
}
