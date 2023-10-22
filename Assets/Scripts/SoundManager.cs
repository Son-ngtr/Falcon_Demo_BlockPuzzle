using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Text soundOnIcon;
    [SerializeField] Text soundOffIcon;

    public AudioSource[] popupSound;

    private bool muted = false;
    void Start()
    {
        soundOnIcon.enabled = true;
        soundOffIcon.enabled = false;
        if(GameObject.FindGameObjectWithTag("PopupSound") != null)
        {
            popupSound = GameObject.FindGameObjectWithTag("PopupSound").GetComponentsInChildren<AudioSource>();
        }
        

        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (muted == false)
        {
            soundOffIcon.enabled = false;
            soundOnIcon.enabled = true;
        }
        else
        {
            soundOffIcon.enabled = true;
            soundOnIcon.enabled = false;
        }
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }


    public void playThisSoundEffect()
    {
        if(popupSound != null)
        {
            popupSound[0].Play();
        }
    }
}
