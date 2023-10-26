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

    private bool _muted = false;
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
        AudioListener.pause = _muted;
    }

    // Cập nhật âm thanh và nút
    public void OnButtonPress()
    {
        if (_muted == false)
        {
            _muted = true;
            AudioListener.pause = true;
        }
        else
        {
            _muted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (_muted == false)
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
        _muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", _muted ? 1 : 0);
    }

    // Chạy âm thanh
    public void playThisSoundEffect()
    {
        if(popupSound != null)
        {
            popupSound[0].Play();
        }
    }
}
