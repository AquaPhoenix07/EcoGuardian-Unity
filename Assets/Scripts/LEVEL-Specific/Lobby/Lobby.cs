using System;
using CORE.Game_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    private SoundManager soundManager;
    [Header("UI References")]
    public Toggle musicToggle;

    public GameObject levelImage;
    public void Start()
    {
        soundManager = SoundManager.Instance;
        if (musicToggle != null)
        {
            bool currentMusicState = soundManager.isBackgroundMusicOn;
            // Dùng dấu ! vì logic là nút Mute (Tắt tiếng)
            musicToggle.SetIsOnWithoutNotify(!currentMusicState);
        }
    }

    public void Update()
    {
        
    }

    public void LevelClicked()
    {
        soundManager.StartCoroutine("PlayButtonClicked");
    }

    public void BackgroundMusicToggle(bool isOn)
    {
        soundManager.BackgroundMusicToggle(isOn);
    }
    //NÚT PLAY
    //Ấn nút home để thoát màn hình chọn màn
    public void HomeButtonClicked()
    {
        levelImage.SetActive(false);
    }
    //Ấn nút chơi
    public void PlayButtonClicked()
    {
        levelImage.SetActive(true);
    }
    
}